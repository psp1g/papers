using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psp_papers_mod.src.Utils;
internal class SplitUsername {

    private static bool IsVowel(char c) {
        return "aeiouyAEIOUY".IndexOf(c) >= 0;
    }

    // starts in the middle, looks for a consonant after a vowel 
    private static int SyllableSplit(string str) {

        int startIdx = str.Length / 2;
        int idx;

        for (idx = startIdx; idx < startIdx + 3; idx++) {
            if (idx + 1 >= str.Length)
                break;

            if (IsVowel(str[idx]) && IsVowel(str[idx + 1])) {
                continue;
            }

            if (IsVowel(str[idx]) && !IsVowel(str[idx + 1])) {
                idx++;
                break;
            }
        }


        return idx;
    }

    static string Capitalize(string str) {
        if (str == null)
            return null;

        if (str.Length > 1)
            return char.ToUpper(str[0]) + str.Substring(1);

        return str.ToUpper();
    }


    public static (string, string) process(string username) {

        int splitIndex = SyllableSplit(username);
        int revertIndex = splitIndex;


        // nikiTuh -> Tuh, niki 
        int lastCapital = System.Array.FindIndex(username[1..].ToArray(), char.IsUpper);
        if (lastCapital > 0) {

            splitIndex = lastCapital + 1;
        }

        // nikita32 -> 32, nikita
        int lastNumeral = System.Array.FindIndex(username.ToArray(), char.IsNumber);
        if (lastNumeral > 0) {

            splitIndex = lastNumeral;
        }

        // niki_cuh -> cuh, niki
        int hyphenIndex = username.LastIndexOf("-");
        if (hyphenIndex != -1 && hyphenIndex != username.Length) {
            username = username.Remove(hyphenIndex, 1);
            splitIndex = hyphenIndex;
        }

        // niki-cuh -> cuh, niki
        int underscoreIndex = username.LastIndexOf("_");
        if (underscoreIndex != -1 && underscoreIndex != username.Length) {
            username = username.Remove(underscoreIndex, 1);
            splitIndex = underscoreIndex;
        }

        int minLetters = 2;
        if (splitIndex < minLetters || username.Length - splitIndex < minLetters) {
            splitIndex = username.Length / 2;
        }

        // uni1g -> 1g, uni
        if (username.EndsWith("1g")) {
            Console.WriteLine("1g");

            splitIndex = username.LastIndexOf("1g");
        }

        string first = username[..splitIndex];
        string last = username[splitIndex..];

        // whether first letters should be capitalized is rather debatable
        //first = Capitalize(first);
        //last = Capitalize(last);

        return (first, last);
    }




}
