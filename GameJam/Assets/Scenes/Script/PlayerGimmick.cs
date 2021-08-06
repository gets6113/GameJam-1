using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGimmick : MonoBehaviour
{

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Lava")//�}�O�}�q�b�g��
        {
            rb.position = last.LastPosition;
        }
    }


}
