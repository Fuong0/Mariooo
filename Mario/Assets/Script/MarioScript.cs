using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioScript : MonoBehaviour
{
    private float VanToc=7;
    private float VanTocToiDa=12;//Van toc toi da khi giu phim Z - Chay nhanh
    private float TocDo=0; //Toc Do cua mario
    private bool DuoiDat=true;//Kiem tra mario co o duoi dat ko
    private float NhayCao=460; //Lay toc do nhay cua Mario
    private float NhayThap=5;//Mario nhay thap, nhan nhanh va buong phim X
    private float RoiXuong=5;// Vat ly giup Mario roi xuong nhanh hon
    private bool ChuyenHuong=false;//Xem mario co dang chuyen huong hay ko
    private Rigidbody2D r2d;
    private bool QuayPhai = true; // Kiem tra xem mario quay ve huong nao
    private float KTGiuPhim = 0.2f;
    private float TGGiuPhim = 0;
    private Animator HoatHoa;


    //Hien thi cap do va do lon cua Mario
    public int CapDo = 0;
    public bool BienHinh=false;
    //Vi tri luc Mariochet
    private Vector2 VitriChet;
    private AudioSource AmThanh;

    // Start is called before the first frame update
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        HoatHoa = GetComponent<Animator>();
        AmThanh = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HoatHoa.SetFloat("TocDo", TocDo);
        HoatHoa.SetBool("DuoiDat", DuoiDat);
        HoatHoa.SetBool("ChuyenHuong",  ChuyenHuong);
        NhayLen();
        BanDanVaTangToc();
        if (BienHinh == true)
        {
            switch(CapDo)
            {
                case 0:
                    {
                        StartCoroutine(MarioThuNho());
                        TaoAmThanh("MarioNhoDi");
                        BienHinh = false;
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(MarioAnNam());
                        BienHinh = false;
                        TaoAmThanh("MarioLonLen");
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(MarioAnHoa());
                        TaoAmThanh("MarioAnHoa");
                        BienHinh = false;
                        break;
                    }
                default:BienHinh = false; 
                break;
            }
        }

        if(gameObject.transform.position.y<-10f)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        DiChuyen();
    }

    void DiChuyen()
    {
        //Chon phim di chuyen cho Mario( phim mui ten phai va mui ten trai hoac a & d)
        float PhimNhanPhaiTrai = Input.GetAxis("Horizontal");
        r2d.velocity=new Vector2(VanToc*PhimNhanPhaiTrai,r2d.velocity.y);
        TocDo = Mathf.Abs( VanToc * PhimNhanPhaiTrai);
        if (PhimNhanPhaiTrai > 0 && ! QuayPhai) HuongMatMario();
        if (PhimNhanPhaiTrai < 0 && QuayPhai) HuongMatMario();
    }
    void HuongMatMario()
    {
        //Neu mario ko quay phai thi
        QuayPhai = !QuayPhai;
        Vector2 HuongQuay = transform.localScale;
        HuongQuay.x *= -1;
        transform.localScale = HuongQuay;
        if(TocDo > 0.5) StartCoroutine(MarioChuyenHuong());
    }
    void NhayLen()
    {
        if (Input.GetKeyDown(KeyCode.W) && DuoiDat == true)
        {
            r2d.AddForce((Vector2.up) * NhayCao);
            TaoAmThanh("MarioNhay");
            DuoiDat = false;
        }
        //Ap dung vat ly cho Mario roi xuong nhanh hon
        if (r2d.velocity.y < 0)
        {
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (RoiXuong - 1) * Time.deltaTime;
        }
        else if(r2d.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (NhayThap - 1) * Time.deltaTime;
        }
        

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "nen dat")
        {
            DuoiDat = true;
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "nen dat")
        {
            DuoiDat = true;
        }
    }
    IEnumerator MarioChuyenHuong()
    {
        ChuyenHuong = true;
        yield return new WaitForSeconds(0.2f);
        ChuyenHuong = false;
    }
    //Ban dan va chay nhanh hon
    void BanDanVaTangToc()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            TGGiuPhim += Time.deltaTime;
            if (TGGiuPhim < KTGiuPhim)
            {
                print("Dang ban dan");
            }
            else
            {
                VanToc = VanToc * 1.005f;
                if (VanToc > VanTocToiDa) VanToc = VanTocToiDa;
            }
        }
        if(Input.GetKeyUp(KeyCode.Z))
        {
            VanToc = 7;
            TGGiuPhim = 0;
        }
    }
    //Thay doi do lon cua Mario
    IEnumerator MarioAnNam()
    {
        float DoTre = 0.1f;
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);

    }
    IEnumerator MarioAnHoa()
    {
        float DoTre = 0.1f;
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 1);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 1);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 1);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 1);
        yield return new WaitForSeconds(DoTre);
    }
    IEnumerator MarioThuNho()
    {
        float DoTre = 0.1f;
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioNho"), 1);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("MarioLon"), 0);
        HoatHoa.SetLayerWeight(HoatHoa.GetLayerIndex("AnHoa"), 0);
        yield return new WaitForSeconds(DoTre);
    }

    public void TaoAmThanh(string FileAmThanh)
    {
        AmThanh.PlayOneShot(Resources.Load<AudioClip>("Audio/" + FileAmThanh));
    }

    public void MarioChet()
    {
        VitriChet = transform.localPosition;
        GameObject MarioChet = (GameObject)Instantiate(Resources.Load("Prefabs/MarioChet"));
        MarioChet.transform.localPosition = VitriChet;
        Destroy(gameObject);
    }
}
