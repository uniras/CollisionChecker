using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CollisionChecker
{
    /// <summary>
    /// 衝突判定ノード。GameObjectを指定して衝突判定する。
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
    /// 衝突判定ノード。GameObjectの名前を指定して衝突判定する。
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
    /// 衝突判定ノード。タグが設定されているGameObjectを指定して衝突判定する。
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
    /// 衝突判定ノード。タグ名を指定して衝突判定する。
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
    /// 衝突判定ノードで衝突判定をするためのMonoBehaiviurクラスとヘルパメソッド
    /// </summary>
    public class CollisionChecker : MonoBehaviour
    {
        /// <summary>
        /// 何も衝突していないことを指す文字列
        /// </summary>
        public const string NullString = "None";

        /// <summary>
        /// タグが設定されていないことを指す文字列
        /// </summary>
        public const string UntaggedString = "Untagged";

        protected static int _MaxCollisionCount => 10;

        /// <summary>
        /// 衝突したオブジェクト名を記憶しておく最大数
        /// </summary>
        public static int MaxCollisionCount
        {
            get
            {
                return _MaxCollisionCount;
            }
        }

        /// <summary>
        /// 衝突したオブジェクト名とタグ名を記録する内部データクラス
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

            //追加
            internal void Add(string obj, string tag)
            {
                if (Count + 1 == MaxCollisionCount) return;
                ObjectName[Count] = obj;
                TagName[Count] = tag;
                Count++;
            }

            //クリア
            internal void Clear()
            {
                Count = 0;
            }

            //オブジェクト名で判定
            internal bool CheckObjectName(string name)
            {
                return Check(ObjectName, name, Count);
            }

            //タグ名で判定
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

        //オブジェクトごとに記録するため辞書型
        private static Dictionary<string, CollisionCheckerData> _ColObjDic = null;

        /// <summary>
        /// オブジェクトのシーン名とオブジェクト名の取得。
        /// 衝突記録用
        /// </summary>
        /// <param name="obj">ゲームオブジェクト</param>
        /// <returns>"シーン名::オブジェクト名"という形式の文字列</returns>
        public static string GetSceneNameAndObjectName(GameObject obj)
        {
            if (obj == null) return NullString + "::" + NullString;
            return obj.scene.name + "::" + obj.name;
        }

        /// <summary>
        /// オブジェクト名の取得とNullチェック
        /// </summary>
        /// <param name="obj">ゲームオブジェクト</param>
        /// <returns>ゲームオブジェクト名、nullの場合はNullStringフィールドの値(None)</returns>
        public static string GetObjectName(GameObject obj)
        {
            if (obj == null) return NullString;
            return obj.name;
        }

        /// <summary>
        /// タグ名の取得とNullチェック
        /// </summary>
        /// <param name="obj">ゲームオブジェクト</param>
        /// <returns>ゲームオブジェクトに設定されてるタグ名。nullの場合はUntaggedStringフィールドの値(Untagged)</returns>
        public static string GetTagName(GameObject obj)
        {
            if (obj == null) return UntaggedString;
            return obj.tag;
        }

        /// <summary>
        /// 準備処理。
        /// 必要なインスタンスの存在をチェックして必要なら作成。
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
        /// 衝突したオブジェクト名の取得
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <param name="index">インデックス</param>
        /// <returns>あればオブジェクト名、なければNullStringフィールドの値(None)</returns>
        public static string GetCollisionName(GameObject target, int index = 0)
        {
            if (target == null) return NullString;
            Preparation(target);
            if (_ColObjDic[GetSceneNameAndObjectName(target)].Count <= index) return NullString;
            return _ColObjDic[GetSceneNameAndObjectName(target)].ObjectName[index];
        }

        /// <summary>
        /// 衝突したタグ名の取得
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <param name="index">インデックス</param>
        /// <returns>あればタグ名、なければUntaggedStringフィールドの値(Untagged)</returns>
        public static string GetCollisionTagName(GameObject target, int index = 0)
        {
            if (target == null) return UntaggedString;
            Preparation(target);
            if (_ColObjDic[GetSceneNameAndObjectName(target)].Count <= index) return UntaggedString;
            return _ColObjDic[GetSceneNameAndObjectName(target)].TagName[index];
        }

        /// <summary>
        /// 衝突したオブジェクト名のリストを取得
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <returns>オブジェクト名のリスト</returns>
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
        /// 衝突したタグ名のリストを取得
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <returns>タグ名のリスト</returns>
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
        /// 衝突記録をクリアする
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        public static void ClearCollisionName(GameObject target)
        {
            if (target == null) return;
            Preparation(target);
            _ColObjDic[GetSceneNameAndObjectName(target)].Clear();
        }

        /// <summary>
        /// 衝突したとして衝突記録に追加する
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <param name="obj">衝突したゲームオブジェクト</param>
        public static void SetCollisionName(GameObject target, GameObject obj)
        {
            if (target == null) return;
            Preparation(target);
            _ColObjDic[GetSceneNameAndObjectName(target)].Add(GetObjectName(obj), GetTagName(obj));
        }

        /// <summary>
        /// ゲームオブジェクトから衝突判定
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <param name="obj">衝突確認するゲームオブジェクト</param>
        /// <returns>衝突したかどうかのbool値</returns>
        public static bool CheckCollisionObject(GameObject target, GameObject obj)
        {
            return CheckCollisionObject(target, GetObjectName(obj));
        }

        /// <summary>
        /// オブジェクト名から衝突判定
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <param name="name">衝突確認するゲームオブジェクトの名前</param>
        /// <returns>衝突したかどうかのbool値</returns>
        public static bool CheckCollisionObject(GameObject target, string name)
        {
            if (target == null) return false;
            Preparation(target);
            return _ColObjDic[GetSceneNameAndObjectName(target)].CheckObjectName(name);
        }

        /// <summary>
        /// オブジェクトのタグ名から衝突判定
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <param name="obj">衝突確認するゲームオブジェクト</param>
        /// <returns>衝突したかどうかのbool値</returns>
        public static bool CheckCollisionTagName(GameObject target, GameObject obj)
        {
            return CheckCollisionTagName(target, GetTagName(obj));
        }

        /// <summary>
        /// タグ名から衝突判定
        /// </summary>
        /// <param name="target">衝突判定しているゲームオブジェクト</param>
        /// <param name="name">衝突確認するタグ名</param>
        /// <returns>衝突したかどうかのbool値</returns>
        public static bool CheckCollisionTagName(GameObject target, string name)
        {
            if (target == null) return false;
            Preparation(target);
            return _ColObjDic[GetSceneNameAndObjectName(target)].CheckTagName(name);
        }

        //
        //衝突したオブジェクトを記録するためのイベントハンドラ
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
