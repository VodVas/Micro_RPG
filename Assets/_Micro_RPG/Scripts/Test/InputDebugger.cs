//using UnityEngine;
//using UnityEngine.InputSystem;

//public class InputDebugger : MonoBehaviour
//{
//    [SerializeField] private InputActionReference _attackAction;

//    private bool _isAttacking;

//    private void Awake()
//    {
//        if (_attackAction == null)
//        {
//            Debug.Log("Attack action not assigned", this);
//            enabled = false;
//            return;
//        }

//        _attackAction.action.Enable();
//    }

//    private void OnEnable()
//    {
//        _attackAction.action.performed += OnAttackPerformed;
//        _attackAction.action.canceled += OnAttackCanceled;
//    }

//    private void OnDisable()
//    {
//        _attackAction.action.performed -= OnAttackPerformed;
//        _attackAction.action.canceled -= OnAttackCanceled;
//    }

//    private void OnAttackPerformed(InputAction.CallbackContext context)
//    {
//        _isAttacking = true;
//        Debug.Log("Attack button pressed", this);
//        // Здесь можно вызвать логику атаки
//        Attack();
//    }

//    private void OnAttackCanceled(InputAction.CallbackContext context)
//    {
//        _isAttacking = false;
//        Debug.Log("Attack button released", this);
//    }

//    private void Attack()
//    {
//        // Реальная логика атаки
//        Debug.Log("Performing attack!", this);
//    }
//}