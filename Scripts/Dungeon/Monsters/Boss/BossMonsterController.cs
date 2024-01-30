using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BossMonster
{
    public class AttackPatternNode
    {
        public int type;
        public float attackDelay;
        public List<AttackPatternNode> nextNodes;
    }
    
    public class BossMonsterController : MonoBehaviour
    {
        public static BossMonsterController instance;

        private enum DamagedState { Idle, Damaged };
        private DamagedState damagedState;

        public int maxHP;
        private int HP;
        public Image HPBar;
        
        private List<AttackPatternNode> attackPatternNodes;
        private int nodeIndex;

        protected Animator anim;
        private SpriteRenderer spriteRenderer;
        protected AudioSource atttackAudioSource;
        protected AudioSource damagedAudioSource;
        public AudioClip audioAttack;
        public AudioClip audioDamaged;
        
        public string[] itemNames;
        public int[] itemCounts;
        public int gold;

        [HideInInspector]
        public GameObject target;

        // 오브젝트 풀링용
        [SerializeField]
        private GameObject[] ObjectPoolingPrefabs;
        private Dictionary<string, GameObject> poolingObjectPrefabs = new Dictionary<string, GameObject>();
        private Dictionary<string, Queue<GameObject>> poolingObjectQueues = new Dictionary<string, Queue<GameObject>>();

        private IEnumerator currentCoroutine;

        #region Initialize

        private void ReadAttackPattern()
        {
            attackPatternNodes = IOManager.instance.ReadJsonFromResources<Dictionary<string, List<AttackPatternNode>>>("Dungeon/KingSlimeAttackPattern")["KingSlimeAttackPattern"];
        }

        private void InitializeObject(int initCount)
        {
            for (int i = 0; i < ObjectPoolingPrefabs.Length; i++)
            {
                poolingObjectPrefabs.Add(ObjectPoolingPrefabs[i].name, ObjectPoolingPrefabs[i]);
                poolingObjectQueues.Add(ObjectPoolingPrefabs[i].name, new Queue<GameObject>());

                for (int j = 0; j < initCount; j++)
                {
                    poolingObjectQueues[ObjectPoolingPrefabs[i].name].Enqueue(CreateNewObject(ObjectPoolingPrefabs[i].name));
                }
            }
        }

        private GameObject CreateNewObject(string objectName)
        {
            var newObj = Instantiate(poolingObjectPrefabs[objectName], transform, true);
            newObj.gameObject.SetActive(false);
            return newObj;
        }

        public GameObject GetObject(string objectName)
        {
            if (poolingObjectQueues[objectName].Count > 0)
            {
                var obj = poolingObjectQueues[objectName].Dequeue();
                obj.transform.SetParent(null);
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var newObj = CreateNewObject(objectName);
                newObj.gameObject.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }
        }

        public void ReturnObject(string objectName, GameObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            poolingObjectQueues[objectName].Enqueue(obj);
        }

        #endregion

        private void ChangeAttackPattern()
        {
            nodeIndex++;
            var currentNode = attackPatternNodes[nodeIndex];

            switch (currentNode.type)
            {
                case 1:
                    currentCoroutine = ShootingPattern1(currentNode);
                    break;

                case 2:
                    currentCoroutine = ShootingPattern2(currentNode);
                    break;

                case 3:
                    currentCoroutine = ShootingPattern3(currentNode);
                    break;
            }
            
            StartCoroutine(currentCoroutine);
        }

        // 무작위 결정
        protected virtual void DecideNextNode(AttackPatternNode node)
        {
            if (node.nextNodes.Count > 0)
            {
                int random = Random.Range(0, 2);

                switch (node.nextNodes[random].type)
                {
                    case 1:
                        currentCoroutine = ShootingPattern1(node);
                        break;

                    case 2:
                        currentCoroutine = ShootingPattern2(node);
                        break;

                    case 3:
                        currentCoroutine = ShootingPattern3(node);
                        break;
                }
                
                StartCoroutine(currentCoroutine);
            }
            else
            {
                ChangeAttackPattern();
            }
        }

        protected virtual IEnumerator ShootingPattern1(AttackPatternNode node)
        {
            yield break;
        }

        protected virtual IEnumerator ShootingPattern2(AttackPatternNode node)
        {
            yield break;
        }

        protected virtual IEnumerator ShootingPattern3(AttackPatternNode node)
        {
            yield break;
        }

        #region Damaged

        private bool isDead = false;
        public void Damaged(GameObject bullet)
        {
            damagedAudioSource.Play();
            if (isDead) return;

            HP -= bullet.GetComponent<Bullet>().damage;
            HPBar.fillAmount = (float)HP / maxHP;

            if (HP <= 0)
            {
                /*DungeonManager.instance.ClearDungeon();
                Time.timeScale = 0;*/
                isDead = true;
                Dead();
            }

            damagedAudioSource.clip = audioDamaged;
            damagedAudioSource.Play();
            /*if (damagedState == DamagedState.Idle)
            {


                //damagedState = DamagedState.Damaged;
            }*/
        }

        private void Dead()
        {
            foreach (var t in gameObject.GetComponents<QuestTrigger>())
            {
                t.OnQuestTrigger();
            }
            
            anim.SetTrigger("Dead");
            StopCoroutine(currentCoroutine);
            DungeonManager.instance.adjustGold = gold;
            DungeonManager.instance.isAllCleared = true;
        }

        public float radius;
        public void DropItem()
        {
            StartCoroutine(DropItemDelay());
        }

        IEnumerator DropItemDelay()
        {
            for (int i = 0; i < itemNames.Length; i++)
            {
                var randomPosition = Random.insideUnitCircle * radius;
                var tempItem = Instantiate(DungeonManager.instance.bossDropItemPrefab, transform.position + new Vector3(randomPosition.x, randomPosition.y, 0) * radius, Quaternion.Euler(0, 0, 0));
                tempItem.GetComponent<BossItemController>().Initialize(itemNames[i], itemCounts[i]);

                yield return new WaitForSeconds(0.05f);
            }
            
            gameObject.SetActive(false);
            DungeonManager.instance.eagle.SetActive(true);
        }

        #endregion


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                    Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            target = DungeonManager.instance.player;
            nodeIndex = -1;

            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            HP = maxHP;
            
            atttackAudioSource = GetComponents<AudioSource>()[0];
            atttackAudioSource.clip = audioAttack;
            atttackAudioSource.volume = GameManager.instance.settings.soundEffectsVolume;
            
            damagedAudioSource = GetComponents<AudioSource>()[1];
            damagedAudioSource.clip = audioDamaged;
            damagedAudioSource.volume = GameManager.instance.settings.soundEffectsVolume;

            ReadAttackPattern();
            InitializeObject(10);

            StartCoroutine(StartDelay());
        }

        IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(2f);

            ChangeAttackPattern();
        }
    }
}
