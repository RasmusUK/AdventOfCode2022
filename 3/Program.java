import java.io.BufferedReader;
import java.io.FileReader;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.HashSet;
import java.util.List;
import java.util.stream.Collector;
import java.util.stream.Collectors;

class Program {
    public static void main(String[] args){
        String line;
        int part1 = 0;
        int part2 = 0;
        try(BufferedReader reader = new BufferedReader(new FileReader(args[0])))
        {
            while ((line = reader.readLine()) != null) {
                part1 += getValueOfCharacter(getCharInBoth(line));
            }
        }
        catch(Exception e){}
        try(BufferedReader reader = new BufferedReader(new FileReader(args[0])))
        {
            while ((line = reader.readLine()) != null) {
                String s1 = line;
                String s2 = reader.readLine();
                String s3 = reader.readLine();
                part2 += getValueOfCharacter(getCharInThree(s1, s2, s3));
            }
        }
        catch(Exception e){}
        System.out.println("Part 1: " + part1);
        System.out.println("Part 2: " + part2);
    }
    private static int getValueOfCharacter(char c)
    {
        if(Character.isUpperCase(c)) return c - 38;
        return c - 96;
    }
    private static char getCharInBoth(String input)
    {
        var fst = stringToCharList(input.substring(0,input.length()/2));
        var snd = stringToCharList(input.substring(input.length()/2,input.length()));
        fst.retainAll(snd);
        return fst.get(0); 
    }
    private static List<Character> stringToCharList(String input)
    {
        var list = new ArrayList<Character>();
        for (var c : input.toCharArray()) {
            list.add(c);
        }
        return list;
    }
    private static char getCharInThree(String s1, String s2, String s3)
    {
        var s = stringToCharList(s1);
        s.retainAll(stringToCharList(s2));
        s.retainAll(stringToCharList(s3));
        return s.get(0); 
    }
}
