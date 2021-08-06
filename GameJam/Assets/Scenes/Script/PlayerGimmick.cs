using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimmick : MonoBehaviour
{

    //�ړ�
    private Vector2 EndPosi;//���B�n�_
    private Vector2 StartPosi;//�J�n�n�_

    [SerializeField] int MoveFlame;//�ړ��t���[��
    private int FlameCount = 0;

    enum HIT : int
    {
        NONE = 0,
        LAVA = 1,
        NEEDLE = 2,
        ICE = 3,
    }
    private int HitFlag = ((int)HIT.NONE);

    //�K�v�Ȃ���
    private Rigidbody2D rb;//����
    private PlayerLastField last;//�Ō�̒��n�_

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        last = this.GetComponent<PlayerLastField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (HitFlag != ((int)HIT.NONE))
        {
            rb.position = Move(StartPosi, EndPosi, (float)FlameCount / (float)MoveFlame);
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);

            FlameCount++;

            if (FlameCount == MoveFlame)
            {
                HitFlag = ((int)HIT.NONE);
                FlameCount = 0;
                rb.position = EndPosi;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lava")//�}�O�}�q�b�g��
        {
            EndPosi = last.LastPosition;
            StartPosi = rb.position;
            last.LastPosiFlag = false;
            HitFlag = ((int)HIT.LAVA);
        }
    }

    //�ړ��֐�
    //Time��0.0f~1.0f
    //Start�n�_
    //End�I�_�@
    private Vector2 Move(Vector2 Start, Vector2 End, float Time)
    {
        Vector2 set = Start + ((End - Start) * Time);

        return set;
    }


    

}
