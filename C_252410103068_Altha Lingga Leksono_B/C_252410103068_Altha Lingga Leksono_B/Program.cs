using System;
using System.Collections.Generic;

namespace MAFIAGALON
{
    public class Agen
    {
        public string IdAgen { get; set; }
        public string NamaAgen { get; set; }
        public int StokAgen { get; private set; }

        public Agen(string idAgen, string namaAgen, int stokAgen)
        {
            IdAgen = idAgen;
            NamaAgen = namaAgen;
            StokAgen = stokAgen < 0 ? 0 : stokAgen;
        }

        public void TambahStok(int jumlah)
        {
            if (jumlah > 0)
            {
                StokAgen += jumlah;
            }
        }

        public void TampilInfo()
        {
            Console.WriteLine($"[AGEN] ID: {IdAgen} | Nama: {NamaAgen} | Stok: {StokAgen} galon");
        }
    }

    public abstract class ProdukGalon
    {
        private string idProduk;
        private string namaProduk;
        private int stokGudang;

        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }

        public int StokGudang
        {
            get { return stokGudang; }
            private set
            {
                if (value < 0)
                {
                    Console.WriteLine("Stok gudang tidak boleh negatif! Diset ke 0.");
                    stokGudang = 0;
                }
                else
                {
                    stokGudang = value;
                }
            }
        }

        protected ProdukGalon(string idProduk, string namaProduk, int stokGudang)
        {
            IdProduk = idProduk;
            NamaProduk = namaProduk;
            StokGudang = stokGudang;
        }

        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                return false;
            }
            if (jumlah > stokGudang)
            {
                return false;
            }

            stokGudang -= jumlah;
            return true;
        }

        public abstract int HitungBiaya(int jumlah);

        public virtual void TampilInfo()
        {
            Console.WriteLine($"[PRODUK] ID: {IdProduk} | Nama: {NamaProduk} | Stok Gudang: {StokGudang} galon");
        }
    }

    public class GalonAkua : ProdukGalon
    {
        private const int HargaPerGalon = 20000;

        public GalonAkua(string idProduk, string namaProduk, int stokGudang)
            : base(idProduk, namaProduk, stokGudang)
        {
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerGalon;
        }

        public override void TampilInfo()
        {
            base.TampilInfo();
            Console.WriteLine($"         Jenis: Galon Akua resmi | Harga: Rp{HargaPerGalon:N0}/galon");
        }
    }

    public class GalonBerkeran : ProdukGalon
    {
        private const int HargaPerGalon = 12000;

        public string JenisKeran { get; set; }

        public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
            : base(idProduk, namaProduk, stokGudang)
        {
            JenisKeran = jenisKeran;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HargaPerGalon;
        }

        public override void TampilInfo()
        {
            base.TampilInfo();
            Console.WriteLine($"         Jenis: Galon Berkeran ({JenisKeran}) | Harga: Rp{HargaPerGalon:N0}/galon");
        }
    }

    public interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    public class DistribusiGalon : DapatDidistribusikan
    {
        public Agen AgenTujuan { get; private set; }
        public ProdukGalon Produk { get; private set; }
        public int Jumlah { get; private set; }

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
        {
            AgenTujuan = agenTujuan;
            Produk = produk;
            Jumlah = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine($"\n>> Mengirim {Jumlah} galon {Produk.NamaProduk} ke agen {AgenTujuan.NamaAgen}...");

            if (Jumlah <= 0)
            {
                Console.WriteLine("   GAGAL: Jumlah galon harus lebih dari 0. Bos tidak suka kiriman kosong.");
                return;
            }

            if (Jumlah > Produk.StokGudang)
            {
                Console.WriteLine($"   GAGAL: Stok gudang tidak cukup (tersedia {Produk.StokGudang}, diminta {Jumlah}).");
                return;
            }

            bool berhasil = Produk.KurangiStok(Jumlah);
            if (!berhasil)
            {
                Console.WriteLine("   GAGAL: Terjadi kesalahan saat mengurangi stok gudang.");
                return;
            }

            AgenTujuan.TambahStok(Jumlah);

            Console.WriteLine($"   Paket berhasil dikirimkan ke agen {AgenTujuan.NamaAgen}. Operasi bersih, tanpa jejak.");
            Console.WriteLine($"   Total biaya: Rp{Produk.HitungBiaya(Jumlah):N0}");
            Console.WriteLine($"   Sisa stok gudang: {Produk.StokGudang} | Stok agen sekarang: {AgenTujuan.StokAgen}");
        }
    }


    public class SindikatGalon
    {
        private List<Agen> daftarAgen = new List<Agen>();
        private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

        public void TambahAgen(Agen agen)
        {
            daftarAgen.Add(agen);
            Console.WriteLine($"Agen {agen.NamaAgen} resmi bergabung dengan sindikat.");
        }

        public void TambahProduk(ProdukGalon produk)
        {
            daftarProduk.Add(produk);
            Console.WriteLine($"Pasokan {produk.NamaProduk} masuk ke gudang pusat.");
        }

        public void TampilkanSindikat()
        {
            Console.WriteLine("\n========== ASET SINDIKAT MAFIA GALON ==========");
            Console.WriteLine("--- Daftar Agen ---");
            foreach (Agen agen in daftarAgen)
            {
                agen.TampilInfo();
            }

            Console.WriteLine("--- Daftar Produk Galon ---");
            foreach (ProdukGalon produk in daftarProduk)
            {
                produk.TampilInfo();
            }
            Console.WriteLine("===============================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== NO. 1: OBJEK AGEN =====");
            Agen agen1 = new Agen("AG01", "Markas Lalad", 10);
            Agen agen2 = new Agen("AG02", "Markas Neil", 5);
            agen1.TampilInfo();
            agen2.TampilInfo();

            Console.WriteLine("\n===== NO. 2 & 3: PRODUK GALON =====");
            GalonAkua akua = new GalonAkua("PG01", "Galon Akua Murni", 100);
            GalonBerkeran berkeran = new GalonBerkeran("PG02", "Galon Berkeran Impor", 50, "Keran Putar");
            akua.TampilInfo();
            berkeran.TampilInfo();

            Console.WriteLine("\n===== NO. 4: POLYMORPHISM =====");
            ProdukGalon[] daftarProduk = { akua, berkeran };
            foreach (ProdukGalon produk in daftarProduk)
            {
                Console.WriteLine($"Biaya 3 galon {produk.NamaProduk}: Rp{produk.HitungBiaya(3):N0}");
            }

            Console.WriteLine("\n===== NO. 6: DISTRIBUSI GALON =====");
            DapatDidistribusikan kirim1 = new DistribusiGalon(agen1, akua, 20);
            DapatDidistribusikan kirim2 = new DistribusiGalon(agen2, berkeran, 0);
            DapatDidistribusikan kirim3 = new DistribusiGalon(agen2, berkeran, 999);
            DapatDidistribusikan kirim4 = new DistribusiGalon(agen2, berkeran, 15);

            kirim1.KirimPaket();
            kirim2.KirimPaket();
            kirim3.KirimPaket();
            kirim4.KirimPaket();

            Console.WriteLine("\n===== NO. 7: SINDIKAT GALON =====");
            SindikatGalon sindikat = new SindikatGalon();
            sindikat.TambahAgen(agen1);
            sindikat.TambahAgen(agen2);
            sindikat.TambahProduk(akua);
            sindikat.TambahProduk(berkeran);
            sindikat.TampilkanSindikat();
        }
    }
}