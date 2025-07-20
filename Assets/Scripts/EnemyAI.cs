using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace KenneyJam2025
{
    public class EnemyAI : MonoBehaviour, IShooter, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        
        [Header("AI Settings")]
        [SerializeField][Range(0, 50)] private float _detectionRange = 15f;
        [SerializeField][Range(0, 50)] private float _targetSwithchingRange = 3f;
        [SerializeField][Range(0, 50)] private float _patrollingRange = 5f;
        [SerializeField] private Vector2 _attachingDodgeRange = new Vector2(1f, 3f); // Range for dodging attacks
        [SerializeField] private float _criticalHealthThreshold = 20f; // Health threshold to consider critical
        [SerializeField][Range(0f,1f)] private float _changeOfFlyingSceneAtCriticalHealth = 0.5f; // Chance to switch to flying state at critical health
        [SerializeField] private Vector2 _flySceneDistanceRange = new Vector2(5f, 10f); // Range for flying distance from the target
        [SerializeField] private float _shootingImprecisionNoise = 0.3f;
        
        [Header("References")]
        [SerializeField] private TMP_Text _debugText;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Gun _equipedGun;
        [SerializeField] private Gun[] _guns;
        
        private AIState _currentState;
        private AIState SearchingForTargetState;
        private AIState AttackingTargetState;
        private AIState FlyState;
        
        private IShooter _targetShooter;
        private Vector3? _targetPosition;
        private bool _isAlive = true; // Flag to check if the AI is alive
        
        private float currentHealth;
        
        private void Start()
        {
            InitializeStates();
            if (_guns.Length == 0)
            {
                Debug.LogError("No guns assigned to PlayerShooting.");
                return;
            }

            for (int i = 0; i < _guns.Length; i++)
            {
                _guns[i].Init(this);
            }
            EquipGun(0);
            ShootersManager.Instance.RegisterShooter(this);
        }

        private void InitializeStates()
        {
            SearchingForTargetState = new AIState("SearchingForTarget", SearchingForTargetState_Enter, SearchingForTargetState_Update, SearchingForTargetState_Exit);
            AttackingTargetState = new AIState("Attacking", AttackingTargetState_Enter, AttackingTargetState_Update, AttackingTargetState_Exit);
            FlyState = new AIState("Flying", FlyState_Enter, FlyState_Update, FlyState_Exit);
            
            _currentState = SearchingForTargetState; // Start with searching for target
        }

        private void Update()
        {
            if (!_isAlive) return; // If AI is not alive, skip updates
            if (GameManager.Instance.GameOver) return;
            if (this.transform.position.y < -6f)
            {
                // If the player falls below a certain height, they die
                Die(null);
            }
            UpdateStateMachine();
        }

        private void UpdateStateMachine()
        {
            if (_currentState == null)
            {
                ChangeState(SearchingForTargetState);
            }
            _currentState.Update?.Invoke();
            _debugText.text = $"{this.gameObject.name}: {_currentState?.Name}";
        }
        
        private void ChangeState(AIState newState)
        {
            _currentState?.Exit?.Invoke();
            _currentState = newState;
            _currentState.Enter?.Invoke();
        }

        private void SearchingForTargetState_Enter()
        {
            _agent.updatePosition = true;
        }
        
        private void SearchingForTargetState_Update()
        {
            if (ShootersManager.Instance.TryGetTargetShooter(this, _detectionRange, out IShooter target))
            {
                _targetShooter = target;
                ChangeState(AttackingTargetState);
                return;
            }

            if (!_targetPosition.HasValue)
            {
                // agent sample patrol position
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _patrollingRange;
                randomDirection.y = 0; // Keep it on the ground plane
                Vector3 randomPosition = transform.position + randomDirection;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, _patrollingRange, NavMesh.AllAreas))
                {
                    _targetPosition = hit.position;
                    _agent.SetDestination(_targetPosition.Value);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, _targetPosition.Value) < 0.5f)
                {
                    // Reached the target position, clear it
                    _targetPosition = null;
                }
            }
        }
        
        private void SearchingForTargetState_Exit()
        {
            
        }
        
        private void AttackingTargetState_Enter()
        {
            _targetPosition = null;
            _agent.updateRotation = false;
            StartShooting();
        }
        
        private void AttackingTargetState_Update()
        {
            if (!_targetPosition.HasValue)
            {
                //chek line of sight
                bool hasLineOfSight = Physics.Raycast(transform.position + Vector3.up * 0.3f, 
                    _targetShooter.Position - transform.position, out RaycastHit hitInfo, 
                    _detectionRange, LayerMask.GetMask("Default"));
                
                if (hasLineOfSight)
                {
                    Vector3 directionToTarget = _targetShooter.Position - transform.position;
                    Vector3 runDirection;
                    if (Random.Range(0, 1) == 0)
                    {
                        runDirection = Vector3.Cross(Vector3.up, directionToTarget).normalized;
                    }
                    else
                    {
                        runDirection = Vector3.Cross(directionToTarget, Vector3.up).normalized;
                    }

                    Vector3 position = transform.position +
                                       runDirection * Random.Range(_attachingDodgeRange.x, _attachingDodgeRange.y);
                    if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        _targetPosition = hit.position;
                        _agent.SetDestination(_targetPosition.Value);
                    }
                }
                else
                {
                    //try to get closer to the target shooter
                    Vector3 directionAwayFromTarget = (_targetShooter.Position - transform.position).normalized;
                    Vector3 position = _targetShooter.Position + directionAwayFromTarget * 
                        Random.Range(_attachingDodgeRange.x, _attachingDodgeRange.y);
                    if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    {
                        _targetPosition = hit.position;
                        _agent.SetDestination(_targetPosition.Value);
                    }
                }
            }
            else
            {
                // Move towards the target position but facing the target shooter
                if (Vector3.Distance(transform.position, _targetPosition.Value) < 0.5f)
                {
                    // Reached the target position, clear it
                    _targetPosition = null;

                    // if it's too far away we fly away
                    if (Vector3.Distance(transform.position, _targetShooter.Position) > _detectionRange)
                    {
                        ChangeState(FlyState);
                    }
                }
                else
                {
                    Vector3 directionToTarget = (_targetShooter.Position - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(directionToTarget);
                }
            }
        }
        
        private void AttackingTargetState_Exit()
        {
            StopShooting();
        }
        
        private void FlyState_Enter()
        {
            _agent.updateRotation = true;
        }
        
        private void FlyState_Update()
        {
            if (!_targetPosition.HasValue)
            {
                Vector3 directionAwayFromTarget = this.transform.position - _targetShooter.Position;
                directionAwayFromTarget.y = 0;
                directionAwayFromTarget.Normalize();
                Vector3 flyPosition = this.transform.position + directionAwayFromTarget *
                    Random.Range(_flySceneDistanceRange.x, _flySceneDistanceRange.y);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(flyPosition, out hit, _detectionRange, NavMesh.AllAreas))
                {
                    _targetPosition = hit.position;
                    _agent.SetDestination(hit.position);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, _targetPosition.Value) < 0.5f)
                {
                    // Reached the target position, clear it
                    _targetPosition = null;
                    // if still in line of sight of the target, switch back to attacking state
                    if (ShootersManager.Instance.TryGetTargetShooter(this, _targetSwithchingRange, out IShooter target))
                    {
                        _targetShooter = target;
                        ChangeState(AttackingTargetState);
                    }
                    else
                    {
                        ChangeState(SearchingForTargetState);
                    }
                }
            }
        }
        
        private void FlyState_Exit()
        {
            
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.blue;
            Handles.Disc(Quaternion.identity, transform.position, Vector3.up, _detectionRange, false, 0);
            Handles.color = Color.red;
            Handles.Disc(Quaternion.identity, transform.position, Vector3.up, _targetSwithchingRange, false, 0);
            Handles.color = Color.green;
            Handles.Disc(Quaternion.identity, transform.position, Vector3.up, _patrollingRange, false, 0);
            
            if (_targetPosition.HasValue)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_targetPosition.Value, 0.5f);
            }
        }
        #endif
        public string Name => gameObject.name;
        public void OnDamage(float damage, IShooter shooter)
        {
            if (!_isAlive) return; // If AI is already dead, ignore damage
            
            currentHealth -= damage;
            Debug.Log($"{Name} damaged by {damage}. Current health: {currentHealth}");
            GlobalEvents.SomethingDamaged?.Invoke(shooter, this, damage);
            if (currentHealth <= 0)
            {
                Die(shooter);
            }
            else
            {
                CheckCriticalHealth();
            }
        }

        private void Die(IShooter shooter)
        {
            _isAlive = false; // Mark AI as dead
            Debug.Log($"{Name} has died.");
            StopShooting(); // Stop shooting if AI dies
            _agent.isStopped = true; // Stop the agent
            _agent.updatePosition = false; // Disable position updates

            _rigidbody.constraints = RigidbodyConstraints.None;
            //add explosion force
            Vector3 randomPosition = transform.position + Random.insideUnitSphere;
            //make it explode
            _rigidbody.AddExplosionForce(10f, randomPosition, 5f, 1f, ForceMode.Impulse);
            currentHealth = 0f; // Set health to zero
            ShootersManager.Instance.UnregisterShooter(this); // Unregister from the shooters manager
            
            this.DelayedCallInSeconds(() =>
            {
                Destroy(this.gameObject);
            }, 2);
        }

        private void CheckCriticalHealth()
        {
            if (currentHealth <= _criticalHealthThreshold)
            {
                if (Random.Range(0f, 1f) < _changeOfFlyingSceneAtCriticalHealth)
                {
                    ChangeState(FlyState);
                }
            }
        }

        public void OnHeal(float heal)
        {
            throw new NotImplementedException();
        }

        public Vector3 Position => transform.position;
        public GameObject GameObject => gameObject;
        public float ImprecisionNoise => _shootingImprecisionNoise;

        public void EquipGun(int index)
        {
            if (index < 0 || index >= _guns.Length)
            {
                Debug.LogError($"Invalid gun index: {index}. Cannot equip gun.");
                return;
            }

            if (_equipedGun != null)
            {
                _equipedGun.StopShooting(); // Stop the current gun before switching
            }

            _equipedGun = _guns[index];
            _equipedGun.Equip(); // Equip the new gun
            Debug.Log($"Equipped gun: {_equipedGun.name}");
        }

        public void StartShooting()
        {
            if (_equipedGun == null)
            {
                Debug.LogError("No gun equipped. Cannot start shooting.");
                return;
            }

            _equipedGun.StartShooting();
        }

        public void StopShooting()
        {
            if (_equipedGun == null)
            {
                Debug.LogError("No gun equipped. Cannot stop shooting.");
                return;
            }

            _equipedGun.StopShooting();
        }

        public void OnSomethingDamaged(IDamageable target, float damage)
        {
        }
    }

    public class AIState
    {
        public string Name;
        public Action Enter;
        public Action Update;
        public Action Exit;

        public AIState(string name, Action enter, Action update, Action exit)
        {
            Name = name;
            Enter = enter;
            Update = update;
            Exit = exit;
        }
    }
}