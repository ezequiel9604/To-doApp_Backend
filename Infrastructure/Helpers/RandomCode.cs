
namespace Infrastructure.Helpers;

public class RandomCode
{

    private static string[] Alphabet = {
        "A", "B", "C", "D", "E", "F",
        "G", "H", "I", "J", "K", "L",
        "M", "N", "O", "P", "R", "S",
        "T", "U", "V", "W", "X", "Z"
    };

    public static string Generate(int amountOfCharacters)
    {

        string result = "";

        for (int i = 0; i < amountOfCharacters; i++)
        {
            int randomLetter = new Random().Next(0, 23);

            if (randomLetter % 2 == 0)
            {
                result += Alphabet[randomLetter];
            }
            else
            {
                int rand = new Random().Next(0, 9);
                result += rand;
            }   

        }

        return result;
    }

}
