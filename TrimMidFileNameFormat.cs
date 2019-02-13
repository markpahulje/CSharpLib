using System;using System.Linq;
public static class Program
{ 
 private static string seperator = "~"; //must be single character string, tilde is a valid filename char, may want to use a non-valid char for filenames.
 private const int leadingnumbers = 5;  //number of leading characters to build up to
 /// <summary>
 /// Format filename to a length (maxFileLen), by splitting filename into 1/3rd leading and 2/3rds ending until max leading numbers is reached. 
 /// Used for toast nofications less than 41 characters
 /// </summary>
 /// <param name="filename"></param>
 /// <param name="maxFileLen"></param>
 /// <returns></returns>
 public static string TrimMidFileNameFormat(this string filename, int maxFileLen)
 {
 
   Console.Write(string.Format("{0,5:###}", maxFileLen)+" "); //remove line when using in your library

   string ext = filename.GetFileNameExtension(); 
   int extLen = ext.Length;  //includes dot in length 
   string filenamelessext = filename.Substring(0, filename.Length - extLen);
   int filenamelessextLen = filenamelessext.Length;
   int maxroom = Math.Min(leadingnumbers * 2 - 1, filenamelessextLen); 
   int minLen  = 2 + 1 + extLen; //1st char + tilde.length(1) + last char + extLen(min 4) => 7 ie s~p.exe
   int lastLen = 1 + extLen; // last char + extLen(min 4) ie p.exe

   if (filename.Length <= maxFileLen)
   return filename;
   else if (filename.Length < minLen)
    return filename;
   else if (maxFileLen < minLen) //implied && filename.Length > maxFileLen) 
    return filename.Substring(0, 1) + seperator + filename.Substring(filename.Length-lastLen, lastLen);
   else if (maxFileLen >= minLen && maxFileLen < (maxroom + minLen)) //implied && filename.Length > maxFileLen) { //middle
   {
     int room = (maxFileLen - lastLen);    
     int one3rd = (3 + room - 1) / 3; //integer division with round up, old room/3 rounds down.
     int two3rds = room - one3rd;  
     return filename.Substring(0, one3rd) + seperator + filename.Substring(filename.Length-(two3rds + lastLen), (two3rds + lastLen));
   }
   else if (filename.Length > maxFileLen + leadingnumbers + 1) // > 2  + 1 + extLen (min 4)  
    return filename.Substring(0, leadingnumbers) + seperator + filename.Substring(filename.Length - maxFileLen, maxFileLen);
   else
    return filename;
 }
  
 private static Random random = new Random(); //not cryptographically secure
 public static string RandomString(int length) 
 {
  const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^*-.";
  return new string(Enumerable.Repeat(chars, length)
    .Select(s => s[random.Next(s.Length)]).ToArray());
 }
 /// <summary>
 /// Get filename extension, not ext of . only returns empty string, mirrors Path.GetExtension not try/catch issues by metadataconsulting.blogspot.com
 /// </summary>
 /// <param name="s"></param>
 /// <returns></returns>
 public static string GetFileNameExtension(this string s)
 {
  string ext = "";
  int fileExtPos = s.LastIndexOf('.');
  if (fileExtPos > s.LastIndexOf('\\'))
  {
   ext = s.Substring(fileExtPos, s.Length - fileExtPos);
   if (ext.Length <= 1) ext = "";
  }
  else
   ext = "";
  return ext;
 }  

 public static void Main()
 {
  string filename = ""; 
  
  for (int j = 1; j < 41; j++) {
   filename = RandomString(j) + ".exe";
   Console.WriteLine(filename); Console.WriteLine("Length=" + filename.Length); Console.WriteLine();
   for (int i = -1; i < filename.Length+3; i++)
   Console.WriteLine(filename.TrimMidFileNameFormat(i)); //7 min length viable name for shrinked name
   Console.WriteLine();
  }
 }
}
