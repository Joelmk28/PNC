using PNC.Models;

namespace PNC.Services;

public class GeographieRdcService
{
    private readonly List<ProvinceRdc> _provinces;

    public GeographieRdcService()
    {
        _provinces = InitialiserProvincesRdc();
    }

    public List<ProvinceRdc> GetProvinces()
    {
        return _provinces.OrderBy(p => p.Nom).ToList();
    }

    public List<string> GetTerritoires(string provinceNom)
    {
        var province = _provinces.FirstOrDefault(p => p.Nom == provinceNom);
        return province?.Territoires.OrderBy(t => t).ToList() ?? new List<string>();
    }

    public List<string> GetCommunes(string provinceNom)
    {
        var province = _provinces.FirstOrDefault(p => p.Nom == provinceNom);
        return province?.Communes.OrderBy(c => c).ToList() ?? new List<string>();
    }

    public List<string> GetVilles(string provinceNom)
    {
        var province = _provinces.FirstOrDefault(p => p.Nom == provinceNom);
        return province?.Villes.OrderBy(v => v).ToList() ?? new List<string>();
    }

    public List<string> GetTerritoiresEtCommunes(string provinceNom)
    {
        var province = _provinces.FirstOrDefault(p => p.Nom == provinceNom);
        if (province == null) return new List<string>();
        
        var result = new List<string>();
        result.AddRange(province.Territoires.Select(t => $"Territoire de {t}"));
        result.AddRange(province.Communes.Select(c => $"Commune de {c}"));
        result.AddRange(province.Villes.Select(v => $"Ville de {v}"));
        
        return result.OrderBy(x => x).ToList();
    }

    private List<ProvinceRdc> InitialiserProvincesRdc()
    {
        return new List<ProvinceRdc>
        {
            new ProvinceRdc
            {
                Nom = "Kinshasa",
                Territoires = new List<string> { "Mont-Ngafula", "Nsele" },
                Communes = new List<string> 
                { 
                    "Bandalungwa", "Barumbu", "Bumbu", "Gombe", "Kalamu", "Kasa-Vubu", 
                    "Kimbanseke", "Kinshasa Commune", "Kintambo", "Kisenso", "Lemba", 
                    "Limete", "Lingwala", "Makala", "Maluku", "Masina", "Matete", 
                    "Mont-Ngafula", "Ndjili", "Ngaba", "Ngaliema", "Ngiri-Ngiri", 
                    "Nsele", "Selembao"
                },
                Villes = new List<string> 
                { 
                    "Kinshasa", "Kinshasa-Centre", "Kinshasa-Est", "Kinshasa-Ouest", 
                    "Kinshasa-Nord", "Kinshasa-Sud"
                }
            },
            new ProvinceRdc
            {
                Nom = "Bas-Uele",
                Territoires = new List<string> 
                { 
                    "Aketi", "Ango", "Bambesa", "Bondo", "Buta", "Poko" 
                },
                Communes = new List<string> { "Buta" },
                Villes = new List<string> { "Buta", "Aketi", "Bambesa", "Bondo" }
            },
            new ProvinceRdc
            {
                Nom = "Équateur",
                Territoires = new List<string> 
                { 
                    "Basankusu", "Bikoro", "Bomongo", "Équateur", "Ingende", 
                    "Lukolela", "Makanza", "Mbandaka"
                },
                Communes = new List<string> { "Mbandaka", "Wangata" },
                Villes = new List<string> { "Mbandaka", "Basankusu", "Bikoro", "Bomongo", "Lukolela" }
            },
            new ProvinceRdc
            {
                Nom = "Haut-Katanga",
                Territoires = new List<string> 
                { 
                    "Kambove", "Kipushi", "Mitwaba", "Pweto", "Sakania" 
                },
                Communes = new List<string> 
                { 
                    "Annex", "Kampemba", "Katuba", "Kenya", "Lubumbashi", 
                    "Ruashi"
                },
                Villes = new List<string> 
                { 
                    "Lubumbashi", "Likasi", "Kipushi", "Kambove", "Pweto", 
                    "Sakania", "Mitwaba"
                }
            },
            new ProvinceRdc
            {
                Nom = "Haut-Lomami",
                Territoires = new List<string> 
                { 
                    "Bukama", "Kamina", "Kaniama", "Malemba-Nkulu" 
                },
                Communes = new List<string> { "Kamina" },
                Villes = new List<string> { "Kamina", "Bukama", "Kaniama" }
            },
            new ProvinceRdc
            {
                Nom = "Haut-Uele",
                Territoires = new List<string> 
                { 
                    "Dungu", "Faradje", "Niangara", "Rungu", "Wamba", "Watsa" 
                },
                Communes = new List<string> { "Isiro" },
                Villes = new List<string> { "Isiro", "Dungu", "Faradje", "Rungu", "Wamba" }
            },
            new ProvinceRdc
            {
                Nom = "Ituri",
                Territoires = new List<string> 
                { 
                    "Aru", "Djugu", "Irumu", "Mahagi", "Mambasa" 
                },
                Communes = new List<string> { "Bunia" },
                Villes = new List<string> { "Bunia", "Aru", "Djugu", "Mambasa" }
            },
            new ProvinceRdc
            {
                Nom = "Kasaï",
                Territoires = new List<string> 
                { 
                    "Dekese", "Demba", "Dimbelenge", "Ilebo", "Kamonia", 
                    "Luebo", "Mweka"
                },
                Communes = new List<string> { "Kananga", "Katoka", "Ndesha" },
                Villes = new List<string> { "Kananga", "Ilebo", "Luebo", "Mweka", "Demba" }
            },
            new ProvinceRdc
            {
                Nom = "Kasaï-Central",
                Territoires = new List<string> 
                { 
                    "Dibaya", "Kazumba", "Luiza", "Sankuru" 
                },
                Communes = new List<string> { "Kananga" },
                Villes = new List<string> { "Kananga", "Luiza", "Dibaya" }
            },
            new ProvinceRdc
            {
                Nom = "Kasaï-Oriental",
                Territoires = new List<string> 
                { 
                    "Kabeya-Kamwanga", "Katanda", "Lupatapata", "Miabi", 
                    "Tshilenge"
                },
                Communes = new List<string> { "Mbuji-Mayi", "Bipemba", "Diulu", "Kanku", "Muya" },
                Villes = new List<string> { "Mbuji-Mayi", "Kabeya-Kamwanga", "Katanda", "Miabi" }
            },
            new ProvinceRdc
            {
                Nom = "Kongo-Central",
                Territoires = new List<string> 
                { 
                    "Kasangulu", "Kimvula", "Lukula", "Madimba", "Mbanza-Ngungu", 
                    "Seke-Banza", "Songololo", "Tshela"
                },
                //Communes = new List<string> { "Matadi", "Boma" },
                Villes = new List<string> { "Matadi", "Boma", "Mbanza-Ngungu", "Kasangulu", "Lukula" }
            },
            new ProvinceRdc
            {
                Nom = "Kwango",
                Territoires = new List<string> 
                { 
                    "Feshi", "Kahemba", "Kasongo-Lunda", "Kwango", "Popokabaka" 
                },
                Communes = new List<string> { "Kenge" },
                Villes = new List<string> { "Kenge", "Feshi", "Kahemba", "Kasongo-Lunda" }
            },
            new ProvinceRdc
            {
                Nom = "Kwilu",
                Territoires = new List<string> 
                { 
                    "Bagata", "Bulungu", "Gungu", "Idiofa", "Kikwit", 
                    "Masimanimba", "Mwendjila"
                },
                Communes = new List<string> { "Kikwit", "Lukemi", "Nzinda" },
                Villes = new List<string> { "Kikwit", "Idiofa", "Bulungu", "Gungu" }
            },
            new ProvinceRdc
            {
                Nom = "Lomami",
                Territoires = new List<string> 
                { 
                    "Kabinda", "Kamiji", "Lomela", "Lubefu", "Lubao", 
                    "Ngandajika"
                },
                Communes = new List<string> { "Mwene-Ditu" },
                Villes = new List<string> { "Mwene-Ditu", "Kabinda", "Lomela", "Lubao" }
            },
            new ProvinceRdc
            {
                Nom = "Lualaba",
                Territoires = new List<string> 
                { 
                    "Dilolo", "Kapanga", "Mutshatsha", "Sandoa" 
                },
                Communes = new List<string> { "Kolwezi", "Mutshatsha" },
                Villes = new List<string> { "Kolwezi", "Mutshatsha", "Dilolo", "Kapanga" }
            },
            new ProvinceRdc
            {
                Nom = "Mai-Ndombe",
                Territoires = new List<string> 
                { 
                    "Bolobo", "Inongo", "Kiri", "Kutu", "Mushie", 
                    "Oshwe", "Yumbi"
                },
                Communes = new List<string> { "Inongo" },
                Villes = new List<string> { "Inongo", "Bolobo", "Kiri", "Mushie" }
            },
            new ProvinceRdc
            {
                Nom = "Maniema",
                Territoires = new List<string> 
                { 
                    "Kabambare", "Kailo", "Kasongo", "Kibombo", "Lubutu", 
                    "Pangi", "Punia"
                },
                Communes = new List<string> { "Kindu" },
                Villes = new List<string> { "Kindu", "Kasongo", "Kibombo", "Lubutu" }
            },
            new ProvinceRdc
            {
                Nom = "Mongala",
                Territoires = new List<string> 
                { 
                    "Bumba", "Businga", "Lisala", "Yahuma" 
                },
                Communes = new List<string> { "Lisala" },
                Villes = new List<string> { "Lisala", "Bumba", "Businga" }
            },
            new ProvinceRdc
            {
                Nom = "Nord-Kivu",
                Territoires = new List<string> 
                { 
                    "Beni", "Butembo", "Lubero", "Masisi", "Nyiragongo", 
                    "Oicha", "Rutshuru", "Walikale"
                },
                Communes = new List<string> 
                { 
                    "Goma", "Karisimbi", "Nyiragongo", "Butembo","Beni", "Katwa", 
                    "Lubero", "Musienene", "Vulamba"
                },
                Villes = new List<string> 
                { 
                    "Goma", "Butembo", "Beni", "Rutshuru", "Walikale", "Masisi"
                }
            },
            new ProvinceRdc
            {
                Nom = "Nord-Ubangi",
                Territoires = new List<string> 
                { 
                    "Bosobolo", "Gbadolite", "Mobayi-Mbongo", "Yakoma" 
                },
                Communes = new List<string> { "Gbadolite" },
                Villes = new List<string> { "Gbadolite", "Bosobolo", "Mobayi-Mbongo" }
            },
            new ProvinceRdc
            {
                Nom = "Sankuru",
                Territoires = new List<string> 
                { 
                    "Kole", "Lomela", "Lusambo", "Sankuru" 
                },
                Communes = new List<string> { "Lusambo" },
                Villes = new List<string> { "Lusambo", "Kole", "Lomela" }
            },
            new ProvinceRdc
            {
                Nom = "Sud-Kivu",
                Territoires = new List<string> 
                { 
                    "Bukavu", "Fizi", "Idjwi", "Kabare", "Kalehe", 
                    "Mwenga", "Shabunda", "Uvira", "Walungu"
                },
                Communes = new List<string> 
                { 
                    "Bagira", "Bukavu", "Ibanda", "Kadutu", "Uvira", "Kiliba"
                },
                Villes = new List<string> 
                { 
                    "Bukavu", "Uvira", "Fizi", "Kalehe", "Mwenga", "Shabunda"
                }
            },
            new ProvinceRdc
            {
                Nom = "Sud-Ubangi",
                Territoires = new List<string> 
                { 
                    "Budjala", "Gemena", "Kungu", "Libenge" 
                },
                Communes = new List<string> { "Gemena" },
                Villes = new List<string> { "Gemena", "Budjala", "Kungu", "Libenge" }
            },
            new ProvinceRdc
            {
                Nom = "Tanganyika",
                Territoires = new List<string> 
                { 
                    "Kalemie", "Kongolo", "Manono", "Moba", "Nyunzu" 
                },
                Communes = new List<string> { "Kalemie", "Nyemba" },
                Villes = new List<string> { "Kalemie", "Kongolo", "Manono", "Moba" }
            },
            new ProvinceRdc
            {
                Nom = "Tshopo",
                Territoires = new List<string> 
                { 
                    "Basoko", "Isangi", "Opala", "Ubundu", "Yahuma", 
                    "Yangambi", "Kisangani"
                },
                Communes = new List<string> 
                { 
                    "Kabondo", "Kisangani", "Lubunga", "Makiso", "Mangobo", "Tshopo"
                },
                Villes = new List<string> 
                { 
                    "Kisangani", "Yangambi", "Basoko", "Isangi", "Ubundu"
                }
            },
            new ProvinceRdc
            {
                Nom = "Tshuapa",
                Territoires = new List<string> 
                { 
                    "Befale", "Boende", "Djolu", "Ikela", "Monkoto" 
                },
                Communes = new List<string> { "Boende" },
                Villes = new List<string> { "Boende", "Befale", "Djolu", "Ikela" }
            }
        };
    }
}

public class ProvinceRdc
{
    public string Nom { get; set; } = string.Empty;
    public List<string> Territoires { get; set; } = new();
    public List<string> Communes { get; set; } = new();
    public List<string> Villes { get; set; } = new();
}


