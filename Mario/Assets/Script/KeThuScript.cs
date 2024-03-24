using System.Collections;
using UnityEngine;

public class KeThuScript : MonoBehaviour
{
    GameObject Mario;
    bool isInvincible = false; // Biến để đánh dấu trạng thái bất tử của Mario

    private void Awake()
    {
        Mario = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && (collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x < 0))
        {
            if (!isInvincible) // Kiểm tra xem Mario có trong trạng thái bất tử không
            {
                if (Mario.GetComponent<MarioScript>().CapDo >= 0)
                {
                    Mario.GetComponent<MarioScript>().CapDo -= 1;
                    Mario.GetComponent<MarioScript>().BienHinh = true;

                    // Gọi coroutine để đặt trạng thái bất tử trong 1 giây
                    StartCoroutine(MakeInvincible());
                }
                else
                {
                    Mario.GetComponent<MarioScript>().MarioChet();
                }
            }
        }
    }

    IEnumerator MakeInvincible()
    {
        isInvincible = true; // Đặt trạng thái bất tử
        yield return new WaitForSeconds(1f); // Đợi 1 giây
        isInvincible = false; // Hủy trạng thái bất tử
    }
}
