using UnityEngine;

namespace Enemy
{
    public class EnemyParentSC : MonoBehaviour
    {
        protected EnemyCommonSC _Common;
        protected Rigidbody2D _RB;
        protected SpriteRenderer _SR;
        protected Animator _Ani;
        protected void _Caching()
        {
            _RB = GetComponent<Rigidbody2D>();
            _SR = GetComponent<SpriteRenderer>();
            _Common = GetComponent<EnemyCommonSC>();
            _Ani = GetComponent<Animator>();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public virtual void _Appear(){

        }
    }
}
