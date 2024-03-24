using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhoiChuaVatPham : MonoBehaviour
{
    private float DoNayCuaKhoi = 0.5f;
    private float TocDoNay = 4f;
    private bool DuocNay = true;
    private Vector3 VitriLucDau;

    //Cac bien de gan Item 
    public bool ChuaNam=false;
    public bool ChuaXu = false;
    public bool ChuaSao = false;
    //Cho phep so luong xu hien thi
    public int SoLuongXu = 1;

    //Lay cap do cua Mario hien tai
    GameObject Mario;


    private void Awake()
    {
        Mario = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {

    }

    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D col)
    {   //Neu Mario va cham phia duoi khoi
        if (col.collider.tag == "VaCham" && col.contacts[0].normal.y > 0)
        {
            VitriLucDau = transform.position;
            KhoiNayLen();

        }
    }
    void KhoiNayLen()
    {
        if (DuocNay)
        {
            StartCoroutine(KhoiNay());
            DuocNay = false;
            if (ChuaNam) NamVaHoa();
            else if (ChuaXu) HienThiXu();

        }
    }
    IEnumerator KhoiNay()
    {
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + TocDoNay * Time.deltaTime);
            if (transform.localPosition.y >= VitriLucDau.y + DoNayCuaKhoi) break;
            yield return null;
        }
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - TocDoNay * Time.deltaTime);
            if (transform.localPosition.y <= VitriLucDau.y) break;
            Destroy(gameObject);
            GameObject KhoiRong = (GameObject)Instantiate(Resources.Load("Prefabs/KhoiTrong"));
            KhoiRong.transform.position = VitriLucDau;
            yield return null;
        }
    }

    void NamVaHoa()
    {
        int CapDoHienTai = Mario.GetComponent<MarioScript>().CapDo;
        GameObject Nam = null;
        if (CapDoHienTai == 0) Nam = (GameObject)Instantiate(Resources.Load("Prefabs/NamAn"));
        else Nam = (GameObject)Instantiate(Resources.Load("Prefabs/Hoa"));
        Mario.GetComponent<MarioScript>().TaoAmThanh("NamMoc");
        Nam.transform.SetParent(this.transform.parent);
        Nam.transform.localPosition=new Vector2(VitriLucDau.x, VitriLucDau.y+1f);
    }
    void HienThiXu()
    {
        GameObject DongXu= (GameObject)Instantiate(Resources.Load("Prefabs/XuNay"));
        DongXu.transform.SetParent(this.transform.parent);
        DongXu.transform.localPosition = new Vector2(VitriLucDau.x, VitriLucDau.y + 1f);
        StartCoroutine(XuNayLen(DongXu));
    }
    IEnumerator XuNayLen(GameObject DongXu)
    {
        while (true)
        {
            DongXu.transform.localPosition = new Vector2(DongXu.transform.localPosition.x, DongXu.transform.localPosition.y + TocDoNay * Time.deltaTime);
            if (DongXu.transform.localPosition.y >= VitriLucDau.y + 10f) break;
            yield return null;
        }
        while (true)
        {
            transform.localPosition = new Vector2(DongXu.transform.localPosition.x, DongXu.transform.localPosition.y - TocDoNay * Time.deltaTime);
            if (DongXu.transform.localPosition.y <= VitriLucDau.y) break;
            Destroy(DongXu.gameObject);
            
            yield return null;
        }
    }
}