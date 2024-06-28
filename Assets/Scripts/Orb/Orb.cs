using UnityEngine;
using UnityEngine.SceneManagement;

namespace Impaler.Orb
{
    public class Orb : MonoBehaviour, IOrb
    {
        [SerializeField] protected ParticleSystem ballParticles;
        private GameMaster gameMaster;
        [SerializeField] protected bool isMissable; //If the player missed this orb, should he lose a miss?
        [SerializeField] protected float speed;
        private bool inGameScene;
        public float Speed { get { return speed; } set { speed = value; } }

        //Tutorial variables
        private bool inTutorial;
        private Vector2 targetPos;
        //Tutorial variables

        public ParticleSystem BallParticles()
        {
            return ballParticles;
        }
        void Start()
        {
            inGameScene = false;
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                gameMaster = GameMaster.GetGameMaster();
                inGameScene = true;
            }
            inTutorial = false;
        }
        void Update()
        {
            if (inTutorial && Vector2.Distance(targetPos, transform.position) < 0.1f)
                transform.position = Vector2.Lerp(transform.position, targetPos, Speed * Time.deltaTime);
            else transform.position += new Vector3(Speed * Time.deltaTime, 0);
        }

        public void SetTutorial(float commonx)
        {
            targetPos = new Vector2(commonx, transform.position.y);
            inTutorial = true;
        }

        public void TerminateTutorial()
        {
            inTutorial = false;
        }

        private void OnPlayerMissed()
        {
            if (Mathf.Abs(speed) > 0)
            {
                Speed = 0;
                if (isMissable && inGameScene)
                    gameMaster.MissHandler();
            }
         }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IBoundary>() != null)
                OnPlayerMissed();
        }
    }
}
