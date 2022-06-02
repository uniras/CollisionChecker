using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CollisionChecker
{
    /// <summary>
    /// �Փ˔���m�[�h�BGameObject���w�肵�ďՓ˔��肷��B
    /// </summary>
    [UnitCategory("CollisionChecker")]
    [UnitTitle("Check Collision Object")]
    public class CheckObject : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputFlow { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowTrue { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowFalse { get; private set; }

        [DoNotSerialize]
        public ValueInput Target { get; private set; }

        [DoNotSerialize]
        public ValueInput GameObject { get; private set; }

        protected override void Definition()
        {
            InputFlow = ControlInput(nameof(InputFlow), Enter);
            OutputFlowTrue = ControlOutput("True");
            OutputFlowFalse = ControlOutput("False");

            Target = ValueInput<GameObject>(nameof(Target), null);
            GameObject = ValueInput<GameObject>(nameof(GameObject), null);

            Requirement(Target, InputFlow);
            Requirement(GameObject, InputFlow);

            Succession(InputFlow, OutputFlowTrue);
            Succession(InputFlow, OutputFlowFalse);
        }

        public ControlOutput Enter(Flow flow)
        {
            GameObject target = flow.GetValue<GameObject>(Target);
            GameObject gameobj = flow.GetValue<GameObject>(GameObject);
            return CollisionChecker.CheckCollisionObject(target, gameobj) ? OutputFlowTrue : OutputFlowFalse;
        }
    }

    /// <summary>
    /// �Փ˔���m�[�h�BGameObject�̖��O���w�肵�ďՓ˔��肷��B
    /// </summary>
    [UnitCategory("CollisionChecker")]
    [UnitTitle("Check Collision Object Name")]
    public class CheckObjectName : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputFlow { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowTrue { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowFalse { get; private set; }

        [DoNotSerialize]
        public ValueInput Target { get; private set; }

        [DoNotSerialize]
        public ValueInput GameObject { get; private set; }

        protected override void Definition()
        {
            InputFlow = ControlInput(nameof(InputFlow), Enter);
            OutputFlowTrue = ControlOutput("True");
            OutputFlowFalse = ControlOutput("False");

            Target = ValueInput<GameObject>(nameof(Target), null);
            GameObject = ValueInput<string>(nameof(GameObject), "");

            Requirement(Target, InputFlow);
            Requirement(GameObject, InputFlow);

            Succession(InputFlow, OutputFlowTrue);
            Succession(InputFlow, OutputFlowFalse);
        }

        public ControlOutput Enter(Flow flow)
        {
            GameObject target = flow.GetValue<GameObject>(Target);
            string gameobj = flow.GetValue<string>(GameObject);
            return CollisionChecker.CheckCollisionObject(target, gameobj) ? OutputFlowTrue : OutputFlowFalse;
        }
    }

    /// <summary>
    /// �Փ˔���m�[�h�B�^�O���ݒ肳��Ă���GameObject���w�肵�ďՓ˔��肷��B
    /// </summary>
    [UnitCategory("CollisionChecker")]
    [UnitTitle("Check Collision Object Tag")]
    public class CheckObjectTag : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputFlow { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowTrue { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowFalse { get; private set; }

        [DoNotSerialize]
        public ValueInput Target { get; private set; }

        [DoNotSerialize]
        public ValueInput GameObject { get; private set; }

        protected override void Definition()
        {
            InputFlow = ControlInput(nameof(InputFlow), Enter);
            OutputFlowTrue = ControlOutput("True");
            OutputFlowFalse = ControlOutput("False");

            Target = ValueInput<GameObject>(nameof(Target), null);
            GameObject = ValueInput<GameObject>(nameof(GameObject), null);

            Requirement(Target, InputFlow);
            Requirement(GameObject, InputFlow);

            Succession(InputFlow, OutputFlowTrue);
            Succession(InputFlow, OutputFlowFalse);
        }

        public ControlOutput Enter(Flow flow)
        {
            GameObject target = flow.GetValue<GameObject>(Target);
            GameObject gameobj = flow.GetValue<GameObject>(GameObject);
            return CollisionChecker.CheckCollisionTagName(target, gameobj) ? OutputFlowTrue : OutputFlowFalse;
        }
    }

    /// <summary>
    /// �Փ˔���m�[�h�B�^�O�����w�肵�ďՓ˔��肷��B
    /// </summary>
    [UnitCategory("CollisionChecker")]
    [UnitTitle("Check Collision Object Tag Name")]
    public class CheckTagName : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputFlow { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowTrue { get; private set; }

        [DoNotSerialize]
        public ControlOutput OutputFlowFalse { get; private set; }

        [DoNotSerialize]
        public ValueInput Target { get; private set; }

        [DoNotSerialize]
        public ValueInput TagName { get; private set; }

        protected override void Definition()
        {
            InputFlow = ControlInput(nameof(InputFlow), Enter);
            OutputFlowTrue = ControlOutput("True");
            OutputFlowFalse = ControlOutput("False");

            Target = ValueInput<GameObject>(nameof(Target), null);
            TagName = ValueInput<string>(nameof(TagName), "");

            Requirement(Target, InputFlow);
            Requirement(TagName, InputFlow);

            Succession(InputFlow, OutputFlowTrue);
            Succession(InputFlow, OutputFlowFalse);
        }

        public ControlOutput Enter(Flow flow)
        {
            bool result;
            GameObject target = flow.GetValue<GameObject>(Target);
            string tag = flow.GetValue<string>(TagName);

            result = CollisionChecker.CheckCollisionTagName(target, tag);

            return result ? OutputFlowTrue : OutputFlowFalse;
        }
    }

    /// <summary>
    /// �Փ˔���m�[�h�ŏՓ˔�������邽�߂�MonoBehaiviur�N���X�ƃw���p���\�b�h
    /// </summary>
    public class CollisionChecker : MonoBehaviour
    {
        /// <summary>
        /// �����Փ˂��Ă��Ȃ����Ƃ��w��������
        /// </summary>
        public const string NullString = "None";

        /// <summary>
        /// �^�O���ݒ肳��Ă��Ȃ����Ƃ��w��������
        /// </summary>
        public const string UntaggedString = "Untagged";

        protected static int _MaxCollisionCount => 10;

        /// <summary>
        /// �Փ˂����I�u�W�F�N�g�����L�����Ă����ő吔
        /// </summary>
        public static int MaxCollisionCount
        {
            get
            {
                return _MaxCollisionCount;
            }
        }

        /// <summary>
        /// �Փ˂����I�u�W�F�N�g���ƃ^�O�����L�^��������f�[�^�N���X
        /// </summary>
        private class CollisionCheckerData
        {
            internal string[] ObjectName;
            internal string[] TagName;
            internal int Count;

            internal CollisionCheckerData()
            {
                ObjectName = new string[MaxCollisionCount];
                TagName = new string[MaxCollisionCount];
                Count = 0;
            }

            //�ǉ�
            internal void Add(string obj, string tag)
            {
                if (Count + 1 == MaxCollisionCount) return;
                ObjectName[Count] = obj;
                TagName[Count] = tag;
                Count++;
            }

            //�N���A
            internal void Clear()
            {
                Count = 0;
            }

            //�I�u�W�F�N�g���Ŕ���
            internal bool CheckObjectName(string name)
            {
                return Check(ObjectName, name, Count);
            }

            //�^�O���Ŕ���
            internal bool CheckTagName(string name)
            {
                return Check(TagName, name, Count);
            }

            internal static bool Check(string[] dat, string src, int count)
            {
                if (dat == null) return false;
                for (int i = 0; i < count; i++)
                {
                    if (string.Compare(dat[i], src) == 0) return true;
                }
                return false;
            }
        }

        //�I�u�W�F�N�g���ƂɋL�^���邽�ߎ����^
        private static Dictionary<string, CollisionCheckerData> _ColObjDic = null;

        /// <summary>
        /// �I�u�W�F�N�g�̃V�[�����ƃI�u�W�F�N�g���̎擾�B
        /// �ՓˋL�^�p
        /// </summary>
        /// <param name="obj">�Q�[���I�u�W�F�N�g</param>
        /// <returns>"�V�[����::�I�u�W�F�N�g��"�Ƃ����`���̕�����</returns>
        public static string GetSceneNameAndObjectName(GameObject obj)
        {
            if (obj == null) return NullString + "::" + NullString;
            return obj.scene.name + "::" + obj.name;
        }

        /// <summary>
        /// �I�u�W�F�N�g���̎擾��Null�`�F�b�N
        /// </summary>
        /// <param name="obj">�Q�[���I�u�W�F�N�g</param>
        /// <returns>�Q�[���I�u�W�F�N�g���Anull�̏ꍇ��NullString�t�B�[���h�̒l(None)</returns>
        public static string GetObjectName(GameObject obj)
        {
            if (obj == null) return NullString;
            return obj.name;
        }

        /// <summary>
        /// �^�O���̎擾��Null�`�F�b�N
        /// </summary>
        /// <param name="obj">�Q�[���I�u�W�F�N�g</param>
        /// <returns>�Q�[���I�u�W�F�N�g�ɐݒ肳��Ă�^�O���Bnull�̏ꍇ��UntaggedString�t�B�[���h�̒l(Untagged)</returns>
        public static string GetTagName(GameObject obj)
        {
            if (obj == null) return UntaggedString;
            return obj.tag;
        }

        /// <summary>
        /// ���������B
        /// �K�v�ȃC���X�^���X�̑��݂��`�F�b�N���ĕK�v�Ȃ�쐬�B
        /// </summary>
        /// <param name="target"></param>
        public static void Preparation(GameObject target)
        {
            if (target == null) return;
            if (_ColObjDic == null)
            {
                _ColObjDic = new Dictionary<string, CollisionCheckerData>();
            }
            if (!_ColObjDic.ContainsKey(GetSceneNameAndObjectName(target)))
            {
                _ColObjDic.Add(GetSceneNameAndObjectName(target), new CollisionCheckerData());
            }
        }

        /// <summary>
        /// �Փ˂����I�u�W�F�N�g���̎擾
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <param name="index">�C���f�b�N�X</param>
        /// <returns>����΃I�u�W�F�N�g���A�Ȃ����NullString�t�B�[���h�̒l(None)</returns>
        public static string GetCollisionName(GameObject target, int index = 0)
        {
            if (target == null) return NullString;
            Preparation(target);
            if (_ColObjDic[GetSceneNameAndObjectName(target)].Count <= index) return NullString;
            return _ColObjDic[GetSceneNameAndObjectName(target)].ObjectName[index];
        }

        /// <summary>
        /// �Փ˂����^�O���̎擾
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <param name="index">�C���f�b�N�X</param>
        /// <returns>����΃^�O���A�Ȃ����UntaggedString�t�B�[���h�̒l(Untagged)</returns>
        public static string GetCollisionTagName(GameObject target, int index = 0)
        {
            if (target == null) return UntaggedString;
            Preparation(target);
            if (_ColObjDic[GetSceneNameAndObjectName(target)].Count <= index) return UntaggedString;
            return _ColObjDic[GetSceneNameAndObjectName(target)].TagName[index];
        }

        /// <summary>
        /// �Փ˂����I�u�W�F�N�g���̃��X�g���擾
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <returns>�I�u�W�F�N�g���̃��X�g</returns>
        public static List<string> GetCollisionObjectNameList(GameObject target)
        {
            List<string> list = new List<string>();
            if (target == null) return list;
            Preparation(target);
            CollisionCheckerData data = _ColObjDic[GetSceneNameAndObjectName(target)];
            for (int i = 0; i < data.Count; i++)
            {
                list.Add(data.ObjectName[i]);
            }
            return list;
        }

        /// <summary>
        /// �Փ˂����^�O���̃��X�g���擾
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <returns>�^�O���̃��X�g</returns>
        public static List<string> GetCollisionTagNameList(GameObject target)
        {
            List<string> list = new List<string>();
            if (target == null) return list;
            Preparation(target);
            CollisionCheckerData data = _ColObjDic[GetSceneNameAndObjectName(target)];
            for (int i = 0; i < data.Count; i++)
            {
                list.Add(data.TagName[i]);
            }
            return list;
        }

        /// <summary>
        /// �ՓˋL�^���N���A����
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        public static void ClearCollisionName(GameObject target)
        {
            if (target == null) return;
            Preparation(target);
            _ColObjDic[GetSceneNameAndObjectName(target)].Clear();
        }

        /// <summary>
        /// �Փ˂����Ƃ��ďՓˋL�^�ɒǉ�����
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <param name="obj">�Փ˂����Q�[���I�u�W�F�N�g</param>
        public static void SetCollisionName(GameObject target, GameObject obj)
        {
            if (target == null) return;
            Preparation(target);
            _ColObjDic[GetSceneNameAndObjectName(target)].Add(GetObjectName(obj), GetTagName(obj));
        }

        /// <summary>
        /// �Q�[���I�u�W�F�N�g����Փ˔���
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <param name="obj">�Փˊm�F����Q�[���I�u�W�F�N�g</param>
        /// <returns>�Փ˂������ǂ�����bool�l</returns>
        public static bool CheckCollisionObject(GameObject target, GameObject obj)
        {
            return CheckCollisionObject(target, GetObjectName(obj));
        }

        /// <summary>
        /// �I�u�W�F�N�g������Փ˔���
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <param name="name">�Փˊm�F����Q�[���I�u�W�F�N�g�̖��O</param>
        /// <returns>�Փ˂������ǂ�����bool�l</returns>
        public static bool CheckCollisionObject(GameObject target, string name)
        {
            if (target == null) return false;
            Preparation(target);
            return _ColObjDic[GetSceneNameAndObjectName(target)].CheckObjectName(name);
        }

        /// <summary>
        /// �I�u�W�F�N�g�̃^�O������Փ˔���
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <param name="obj">�Փˊm�F����Q�[���I�u�W�F�N�g</param>
        /// <returns>�Փ˂������ǂ�����bool�l</returns>
        public static bool CheckCollisionTagName(GameObject target, GameObject obj)
        {
            return CheckCollisionTagName(target, GetTagName(obj));
        }

        /// <summary>
        /// �^�O������Փ˔���
        /// </summary>
        /// <param name="target">�Փ˔��肵�Ă���Q�[���I�u�W�F�N�g</param>
        /// <param name="name">�Փˊm�F����^�O��</param>
        /// <returns>�Փ˂������ǂ�����bool�l</returns>
        public static bool CheckCollisionTagName(GameObject target, string name)
        {
            if (target == null) return false;
            Preparation(target);
            return _ColObjDic[GetSceneNameAndObjectName(target)].CheckTagName(name);
        }

        //
        //�Փ˂����I�u�W�F�N�g���L�^���邽�߂̃C�x���g�n���h��
        //
        public void OnCollisionEnter(Collision collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }
        public void OnTriggerEnter(Collider collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }

        public void OnCollisionStay(Collision collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }
        public void OnCollisionStay2D(Collision2D collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }
        public void OnTriggerStay(Collider collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            SetCollisionName(this.gameObject, collision.gameObject);
        }

        public void LateUpdate()
        {
            ClearCollisionName(this.gameObject);
        }
    }
}
