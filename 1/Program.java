import java.io.BufferedReader;
import java.io.FileReader;
import java.util.ArrayList;
import java.util.Collections;

class Program {
    public static void main(String[] args){
        String line;
        ArrayList<Integer> calories = new ArrayList<>();
        int acc = 0;
        try(BufferedReader reader = new BufferedReader(new FileReader(args[0])))
        {
            while ((line = reader.readLine()) != null) {
                if(line.isEmpty()) 
                {
                    calories.add(acc);
                    acc = 0;
                }
                else
                {
                    acc += Integer.parseInt(line);
                }
            }
        }
        catch(Exception e){}
        Collections.sort(calories);
        int best = calories.get(calories.size()-1);
        int top3 = calories.get(calories.size()-1) + calories.get(calories.size()-2) + calories.get(calories.size()-3);
        System.out.println("Best: " + best);
        System.out.println("Top 3 total: " + top3);
    }
}