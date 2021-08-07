using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //PlayerGimmick�X�N���v�g
    PlayerGimmick PG;

    //��A���E�̑ϐ��I�u�W�F�N�g(��)
    GameObject[] Cols;

    //Rigidbody2D
    Rigidbody2D rb;

    //�ϐ��������������F�t���p(��)
    Color setColor;

    //Gimmick�擾�p���C���[
    LayerMask Gimmick_Layer;

    //�M�~�b�N�̓����蔻��pbool
    bool bNeedle, bLava, bIce;

    //��������p��bool
    bool bDeth;

    //�ړ��������ʗp
    [SerializeField] float MaxSpeed;//�ō����x
    float direction;

    //�v���C���[�̊p�x�p
    float rotate;

    //�v���C���[�̉�]�p�x�v�Z�p
    float now_Rotate;

    //2�i�W�����v�J�E���g�p
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        PG = this.GetComponent<PlayerGimmick>();

        //�M�~�b�N�ϐ��I�u�W�F�N�g�p�z��
        Cols = new GameObject[3];
        //�E�̃M�~�b�N�ϐ��I�u�W�F�N�g�p
        Cols[0] = transform.Find("Right").gameObject;
        //��̃M�~�b�N�ϐ��I�u�W�F�N�g�p
        Cols[1] = transform.Find("Top").gameObject;
        //���̃M�~�b�N�ϐ��I�u�W�F�N�g�p
        Cols[2] = transform.Find("Left").gameObject;

        //�e�I�u�W�F�N�g�𓧖���(��)
        for (int i = 0; i < Cols.Length; i++)
        {
            Cols[i].GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        }

        //Ray�Ŕ��肷�郌�C���[�ݒ�
        Gimmick_Layer = LayerMask.GetMask("Gimmick");

        //�������Z�R���|�[�l���g�擾
        rb = GetComponent<Rigidbody2D>();

        //�e�M�~�b�Nbool������
        bNeedle = bLava = bIce = false;

        //��������p��bool������
        SetDeth(false);

        //�ړ������p���l������
        direction = 0f;

        //�p�x������
        rotate = 0f;

        //��]�p�x������
        now_Rotate = 0f;

        //�W�����v�J�E���g������
        jumpCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetDeth())
            Move();

        StartCoroutine("Collision");

        //��]�������f 
        this.rb.transform.eulerAngles = new Vector3(0, 0, rotate);
    }

    //�L�[�{�[�h���͓��̏���
    void Move()
    {

        //�X�y�[�X�L�[���������Ƃ��̃W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount != 0)
        {
            //�������Z���Z�b�g
            rb.velocity = Vector2.zero;

            //�W�����v�J�E���g-1
            jumpCount -= 1;

            //�W�����v����
            rb.AddForce(Vector2.up * 800);

            //��]�����J�n
            StartCoroutine("Rotation");
        }

        //�L�[���͂ł̈ړ�����
        if (Input.GetKey(KeyCode.D))
            direction = MaxSpeed;
        else if (Input.GetKey(KeyCode.A))
            direction = -MaxSpeed;

        //���E�ړ�
        rb.position += new Vector2(direction, 0.0f);

        //�^������
        direction *= 0.9f;
    }

    //�W�����v���̉�]�̏���(�R���[�`��)
    IEnumerator Rotation()
    {
        //��]���x�C���^�[�o��������
        float time = 0.0f;

        //��]�p�x�v�Z
        now_Rotate -= 90;

        //360�x�𒴂�����0�ɖ߂�
        if (now_Rotate < -359)
            now_Rotate = 0f;


        //��]�������s
        while (time <= 0.2f)
        {
            //���̊p�x����90�x��]
            rotate = Mathf.Lerp(now_Rotate + 90, now_Rotate, time / 0.1f);

            //�C���^�[�o�����Z
            time += Time.deltaTime;

            yield return 0;
        }
    }

    //Ray�����蔻��擾�p(�J����)&(�R���[�`��)
    IEnumerator Collision()
    {
        //�����蔻��pRay
        RaycastHit2D[] hits = new RaycastHit2D[4];
        //ray�̎n�_
        Vector3 sta_Position = new Vector3(this.rb.transform.position.x, this.rb.transform.position.y);
        //ray�̏I�_
        Vector3[] end_Position = new Vector3[4];
        //ray�̒���
        float end_distance = 0.4f;

        //ray�̊e�I�_�ݒ�(�㉺���E)
        end_Position[0] = sta_Position + rb.transform.right * end_distance;
        end_Position[1] = sta_Position + rb.transform.up * end_distance;
        end_Position[2] = sta_Position - rb.transform.right * end_distance;
        end_Position[3] = sta_Position - rb.transform.up * end_distance;

        //ray�̊e�ݒ�(�㉺���E)
        hits[0] = Physics2D.Linecast(sta_Position, end_Position[0], Gimmick_Layer);
        hits[1] = Physics2D.Linecast(sta_Position, end_Position[1], Gimmick_Layer);
        hits[2] = Physics2D.Linecast(sta_Position, end_Position[2], Gimmick_Layer);
        hits[3] = Physics2D.Linecast(sta_Position, end_Position[3], Gimmick_Layer);

        //�����蔻��m�F�p���[�v
        for (int i = 0; i < hits.Length; i++)
        {
            //i�Ԗڂ�Ray������������
            if (hits[i])
            {
                //�f�o�b�O��Line������p
                Debug.DrawLine(sta_Position, end_Position[i], Color.red);

                //�����ȊO�ɓ���������
                if (0 <= i && i <= 2)
                {
                    //�������������ɐF(�ϐ�)��\��
                    Cols[i].GetComponent<Renderer>().material.color = setColor;
                }
                //�����ɓ���������
                else
                {
                    //�S�Ɖu�폜����
                    for (int j = 0; j < Cols.Length; j++)
                        //�S�Ă̖Ɖu�𓧖���
                        Cols[j].GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0);
                }


                PG.HitGimmick();

                yield return new WaitForSeconds(0.1f);
                //�e�p�x���Z�b�g
                now_Rotate = rotate = 0f;
                //�ebool���Z�b�g
                bNeedle = bLava = bIce = false;
            }
            else
            {
                //�f�o�b�O��Line������p
                Debug.DrawLine(sta_Position, end_Position[i], Color.blue);
            }
        }
    }



    //�������f�pbool�擾�p
    public bool GetDeth()
    {
        return bDeth;
    }
    //�������f�pbool�ݒ�p
    public void SetDeth(bool set)
    {
        bDeth = set;
    }

    //�����蔻��擾�p(��)
    void OnCollisionEnter2D(Collision2D collision)
    {
        //�n�ʂƂ̓����蔻��
        if (collision.gameObject.CompareTag("Ground"))
        {
            //�W�����v�J�E���g���Z�b�g
            jumpCount = 2;
        }

        //�j�Ƃ̓����蔻��
        if (collision.gameObject.CompareTag("Needle"))
        {
            bNeedle = true;
            setColor = Color.gray;
        }
        //�}�O�}�Ƃ̓����蔻��
        else if (collision.gameObject.CompareTag("Lava"))
        {
            bLava = true;
            setColor = Color.red;
        }
        //�X�Ƃ̓����蔻��
        else if (collision.gameObject.CompareTag("Ice"))
        {
            bIce = true;
            setColor = Color.cyan;
        }
    }
}
