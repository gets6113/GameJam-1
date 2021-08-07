using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //PlayerGimmickスクリプト
    PlayerGimmick PG;

    //上、左右の耐性オブジェクト(仮)
    GameObject[] Cols;

    //Rigidbody2D
    Rigidbody2D rb;

    //耐性を持った部分色付け用(仮)
    Color setColor;

    //Gimmick取得用レイヤー
    LayerMask Gimmick_Layer;

    //ギミックの当たり判定用bool
    bool bNeedle, bLava, bIce;

    //生死判定用のbool
    bool bDeth;

    //移動方向判別用
    [SerializeField] float MaxSpeed;//最高速度
    float direction;

    //プレイヤーの角度用
    float rotate;

    //プレイヤーの回転角度計算用
    float now_Rotate;

    //2段ジャンプカウント用
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        PG = this.GetComponent<PlayerGimmick>();

        //ギミック耐性オブジェクト用配列
        Cols = new GameObject[3];
        //右のギミック耐性オブジェクト用
        Cols[0] = transform.Find("Right").gameObject;
        //上のギミック耐性オブジェクト用
        Cols[1] = transform.Find("Top").gameObject;
        //左のギミック耐性オブジェクト用
        Cols[2] = transform.Find("Left").gameObject;

        //各オブジェクトを透明化(仮)
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        }

        //Rayで判定するレイヤー設定
        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        //物理演算コンポーネント取得
        rb = GetComponent<Rigidbody2D>();

        //各ギミックbool初期化
        bNeedle = bLava = bIce = false;

        //生死判定用のbool初期化
        SetDeth(false);

        //移動方向用数値初期化
        direction = 0f;

        //角度初期化
        rotate = 0f;

        //回転角度初期化
        now_Rotate = 0f;

        //ジャンプカウント初期化
        jumpCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetDeth())
            Move();

        StartCoroutine("Collision");

        //回転処理反映 
        this.rb.transform.eulerAngles = new Vector3(0, 0, rotate);
    }

    //キーボード入力等の処理
    void Move()
    {

        //スペースキーを押したときのジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount != 0)
        {
            //物理演算リセット
            rb.velocity = Vector2.zero;

            //ジャンプカウント-1
            jumpCount -= 1;

            //ジャンプ処理
            rb.AddForce(Vector2.up * 800);

            //回転処理開始
            StartCoroutine("Rotation");
        }

        //キー入力での移動処理
        if (Input.GetKey(KeyCode.D))
            direction = MaxSpeed;
        else if (Input.GetKey(KeyCode.A))
            direction = -MaxSpeed;

        //左右移動
        rb.position += new Vector2(direction, 0.0f);

        //疑似完成
        direction *= 0.9f;
    }

    //ジャンプ時の回転の処理(コルーチン)
    IEnumerator Rotation()
    {
        //回転速度インターバル初期化
        float time = 0.0f;

        //回転角度計算
        now_Rotate -= 90;

        //360度を超えたら0に戻す
        if (now_Rotate < -359)
            now_Rotate = 0f;


        //回転処理実行
        while (time <= 0.2f)
        {
            //今の角度から90度回転
            rotate = Mathf.Lerp(now_Rotate + 90, now_Rotate, time / 0.1f);

            //インターバル加算
            time += Time.deltaTime;

            yield return 0;
        }
    }

    //Ray当たり判定取得用(開発中)&(コルーチン)
    IEnumerator Collision()
    {
        //当たり判定用Ray
        RaycastHit2D[] hits = new RaycastHit2D[4];
        //rayの始点
        Vector3 sta_Position = new Vector3(this.rb.transform.position.x, this.rb.transform.position.y);
        //rayの終点
        Vector3[] end_Position = new Vector3[4];
        //rayの長さ
        float end_distance = 0.4f;

        //rayの各終点設定(上下座右)
        end_Position[0] = sta_Position + rb.transform.right * end_distance;
        end_Position[1] = sta_Position + rb.transform.up * end_distance;
        end_Position[2] = sta_Position - rb.transform.right * end_distance;
        end_Position[3] = sta_Position - rb.transform.up * end_distance;

        //rayの各設定(上下座右)
        hits[0] = Physics2D.Linecast(sta_Position, end_Position[0], Gimmick_Layer);
        hits[1] = Physics2D.Linecast(sta_Position, end_Position[1], Gimmick_Layer);
        hits[2] = Physics2D.Linecast(sta_Position, end_Position[2], Gimmick_Layer);
        hits[3] = Physics2D.Linecast(sta_Position, end_Position[3], Gimmick_Layer);

        //当たり判定確認用ループ
        for (int i = 0; i < hits.Length; i++)
        {
            //i番目のRayが当たったか
            if (hits[i])
            {
                //デバッグでLineを見る用
                Debug.DrawLine(sta_Position, end_Position[i], Color.red);

                //足元以外に当たったか
                if (0 <= i && i <= 2)
                {
                    //当たった部分に色(耐性)を表示
                    Cols[i].GetComponent<Renderer>().material.color = setColor;
                }
                //足元に当たったら
                else
                {
                    //全免疫削除処理
                    for (int j = 0; j < Cols.Length; j++)
                        //全ての免疫を透明化
                        Cols[j].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
                }


                PG.HitGimmick();

                yield return new WaitForSeconds(0.1f);
                //各角度リセット
                now_Rotate = rotate = 0f;
                //各boolリセット
                bNeedle = bLava = bIce = false;
            }
            else
            {
                //デバッグでLineを見る用
                Debug.DrawLine(sta_Position, end_Position[i], Color.blue);
            }
        }
    }



    //生死判断用bool取得用
    public bool GetDeth()
    {
        return bDeth;
    }
    //生死判断用bool設定用
    public void SetDeth(bool set)
    {
        bDeth = set;
    }

    //当たり判定取得用(仮)
    void OnCollisionEnter2D(Collision2D collision)
    {
        //地面との当たり判定
        if (collision.gameObject.CompareTag("Ground"))
        {
            //ジャンプカウントリセット
            jumpCount = 2;
        }

        //針との当たり判定
        if (collision.gameObject.CompareTag("Needle"))
        {
            bNeedle = true;
            setColor = Color.gray;
        }
        //マグマとの当たり判定
        else if (collision.gameObject.CompareTag("Lava"))
        {
            bLava = true;
            setColor = Color.red;
        }
        //氷との当たり判定
        else if (collision.gameObject.CompareTag("Ice"))
        {
            bIce = true;
            setColor = Color.cyan;
        }
    }
}
