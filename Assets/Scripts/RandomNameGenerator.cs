using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{


    private static List<string> names = new List<string>
    {
        "Cain",
        "Abel",
        "Isaac",
        "Aran",
        "Alistar",
        "Belos",
        "Camus",
        "Cybele",
        "Danae",
        "Deimos",
        "Erebos",
        "Erato",
        "Eunos",
        "Halia",
        "Helias",
        "Hylas",
        "Iphis",
        "Irae",
        "Irus",
        "Janus",
        "Kampe",
        "Klaude",
        "Kelmis",
        "Klaros",
        "Latona",
        "Leuke",
        "Linos",
        "Lauron",
        "Metis",
        "Mestor",
        "Minos",
        "Musa",
        "Mara",
        "Myrto",
        "Nessos",
        "Neilos",
        "Nyx",
        "Ophis",
        "Perseus",
        "Pisos",
        "Remnus",
        "Rhea",
        "Rhadine",
        "Sallos",
        "Sibyl",
        "Silvanis",
        "Simone",
        "Sol",
        "Typhon",
        "Theon",
        "Ulric",
        "Valentyne",
        "Vannes",
        "Vesta",
        "Zell",
        "Sighard",
        "Sandre",
        "Samson",
        "Robyn",
        "Reeve",
        "Rauffe",
        "Piers",
        "Osric",
        "Orrick",
        "Olyver",
        "Mansel",
        "Morgan",
        "Morys",
        "Madson",
        "Lowell",
        "Kennard",
        "Ivan",
        "Ingram",
        "Gyles",
        "Gwayne",
        "Francis",
        "Folke",
        "Fawkes",
        "Etgar",
        "Erik",
        "Emerick",
        "Carmine",
        "Cameron",
        "Belmont",
        "Baldric",
        "Aldous",
        "Albin",
        "Amice",
        "Clemence",
        "Damaris",
        "Elle",
        "Emery",
        "Emmet",
        "Hester",
        "Isolde",
        "Josiah",
        "Kath",
        "Lewyn",
        "Minerva",
        "Sanche",
        "Sybell",
        "Alaune",
        "Asnan",
        "Arigan",
        "Bafram",
        "Balmin",
        "Aksel",
        "Alek",
        "Anton",
        "Ardin",
        "Benton",
        "Cesare",
        "Elias",
        "Forde",
        "Franz",
        "Iden",
        "Juris",
        "Leroy",
        "Matias",
        "Melanor",
        "Monte",
        "Safra",
        "Shultz",
        "Syrus",
        "Vasil",
        "Yori",
        "Yuwen",
        "Nieve",
        "Mikelle",
        "Maude",
        "Marion",
        "Loren",
        "Becile",
        "Ramiel",
        "Petros"
    };

    private static List<string> usedNames = new List<string>();


    public static string GetRandomName ()
    {

        //If we've used up all the unique names, recycle!
        if (names.Count == 0) { RecycleNames(); }

        int rng = Random.Range(0, names.Count);
        string name = names[rng];
        names.Remove(name);
        usedNames.Add(name);

        return name;

    }


    private static void RecycleNames ()
    {
        names = usedNames;
        usedNames.Clear();
    }
}
