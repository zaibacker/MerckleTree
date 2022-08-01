//Tested on online compiler : https://www.w3schools.com/cs/trycs.php?filename=demo_compiler
//reference : https://developer.bitcoin.org/reference/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MerckleRoot
{
  class Program
  {
    static void Main(string[] args)
    {
                            
        List<string> data = new List<string>() {
							"0.0887",
                            "0.1856",
                            "0.2307",
                            "0.1522",
                            "0.0532",
                            "0.0250",
                            "0.1409",
                            "0.2541",
                            "0.1147",
                            "0.2660",
                            "0.2933",
                            "0.0686"
        };


            Console.WriteLine(MerkleRoot.merkleRoot(data));
    }
  }

    public class MerkleRoot
    {

       public static string merkleRoot(List<string> leaves)
       {
	   //if list is null or empty
            if(leaves == null)
            	return string.Empty;
            if (!leaves.Any())
            	return string.Empty;
                
            //if only one leave remainded, it means we got the root
            // this test must be done before the next one or neverending loop
            if(leaves.Count() == 1)
            	return leaves.First();
                
            //if leaves are odd number we must have an even number. According to bitcoin. we must duplicate the last leaf
            if(leaves.Count() %2 > 0)
            	leaves.Add(leaves.Last());
            
            //Console.WriteLine("leaves count " + leaves.Count());
            
            var parentLeaves = new List<string>();
            //calcul of hash pair by pair using double hash as mentionned in 
	    //https://developer.bitcoin.org/reference/block_chain.html
            for (var index = 0; index < leaves.Count(); index = index +2)
            {
                parentLeaves.Add(HashFunction(leaves[index], leaves[index+1]));
            }
            
            //recursion
            return merkleRoot(parentLeaves);
            
       }
       
       //this function calcul double hash but also convert the transaction hashes from little-endian to big-endian and after hash convert back to little-endian
       //cf https://developer.bitcoin.org/reference/block_chain.html
       	static string HashFunction(string leaf1, string leaf2)
        {
            var pairBytesLeaf1 =  Encoding.ASCII.GetBytes(leaf1);
            var pairBytesLeaf2 =  Encoding.ASCII.GetBytes(leaf2);
            Array.Reverse(pairBytesLeaf1);
            Array.Reverse(pairBytesLeaf2);
            var pairBytes = pairBytesLeaf1.Concat(pairBytesLeaf2).ToArray();
            SHA256 sha256 = SHA256.Create();
            byte[] firstHash = sha256.ComputeHash(pairBytes);
            byte[] hashOfHash = sha256.ComputeHash(firstHash);
            Array.Reverse(hashOfHash);
            return BitConverter.ToString(hashOfHash).Replace("-", "").ToLower();
        }

    }
}
