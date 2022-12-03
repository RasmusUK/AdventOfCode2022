import java.io.BufferedReader;
import java.io.FileReader;
import java.util.HashMap;

import javax.imageio.plugins.tiff.FaxTIFFTagSet;

class Program {
    public static void main(String[] args){
        String line;
        HashMap<String, Integer> roundToScore = new HashMap<>();
        roundToScore.put("A X", 4);
        roundToScore.put("A Y", 8);
        roundToScore.put("A Z", 3);
        roundToScore.put("B X", 1);
        roundToScore.put("B Y", 5);
        roundToScore.put("B Z", 9);
        roundToScore.put("C X", 7);
        roundToScore.put("C Y", 2);
        roundToScore.put("C Z", 6);

        HashMap<String, String> strategy = new HashMap<>();
        strategy.put("A X", "A Z");
        strategy.put("A Y", "A X");
        strategy.put("A Z", "A Y");
        strategy.put("B X", "B X");
        strategy.put("B Y", "B Y");
        strategy.put("B Z", "B Z");
        strategy.put("C X", "C Y");
        strategy.put("C Y", "C Z");
        strategy.put("C Z", "C X");

        int part1 = 0;
        int part2 = 0;

        try(BufferedReader reader = new BufferedReader(new FileReader(args[0])))
        {
            while ((line = reader.readLine()) != null) {
                part1 += roundToScore.get(line);
                part2 += roundToScore.get(strategy.get(line));
            }
        }
        catch(Exception e){}

        System.out.println("Part 1: " + part1);
        System.out.println("Part 2: " + part2);
    }
}
