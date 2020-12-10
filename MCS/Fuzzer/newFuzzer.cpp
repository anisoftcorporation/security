/* 
  The fuzzer by Anirban Bhattacharya for MCSOnline CS 543.
  This is a dumb fuzzer.
  It takes 2 unsigned int 32 inputs
  -First one is prng seed for srand
  -Second one is number of Iteration.
  
  The working directory should contain initial seed in a file named seed.
  
  The fuzzer will change 13% of the seed in every iteration to generate mutate.
  After every 500 iteration, the seed will be updated by appending random 10 characters.
  */
  
#include <iostream>
#include <cstdlib>
#include<string>
#include<vector>
#include <fstream>
#include <sstream>
#include<iterator>

std::vector<char> readSeed()
{
std::ifstream input( "seed", std::ios::binary );

    // copies all data into buffer
       std::vector<char> buffer(std::istreambuf_iterator<char>(input), {});
  //  std::cout<<"From buffer--";
    
    return buffer;
}
void printSeed(std::vector<char> seed)
{
	int size = seed.size();
	for(int i=0;i<size;i++)
	{
  		std::cout<<seed[i]<<std::flush;
	}	
	std::cout<<"\n";
   
}

void mutate(std::vector<char> seed,int random,unsigned int i)
{
  int length = seed.size();
  int thirteen = (length*13)/100;
  int effect;
  if(i <= 256)
  {
    effect = i;
  }
  else
  {
    effect = i%256;
  }
  if(i<length)
   {
        for(unsigned int n=0;n<thirteen;n++)
        {
        if((i+n) < length)
        {
        seed[i+n] = seed[i+n]+random+effect;
        }
        else
        {
         seed[i] = seed[i]+random+effect;
        }
        }
        
   }
   else
   {
         int ptr = i%length;
         
        for(unsigned int n=0;n<thirteen;n++)
        {
        if((ptr+n) < length)
        {
        seed[ptr+n] = seed[ptr+n]+random+effect;
        }
        else
        {
         seed[ptr] = seed[ptr]+random+effect;
        }
        }
        
   }
   printSeed(seed);

}

std::vector<char> append_seed(int m,int itr,std::vector<char> vec)

{
    //int val = m/500;
    
    for(int i=0;i<(10);i++)
    {
        srand(m+i+itr);
        int coup = rand();
        vec.push_back(char(coup%256));
       
    }
   // std::cout<<"Done..for "<<m<<":"<<result<<"\n";
    return vec;
}

int main(int argc, char **argv)

{
    unsigned int prng_seed;
    unsigned int itr;
    prng_seed = atoi(argv[1]);
    itr =  atoi(argv[2]);
    srand(prng_seed);
    int random = rand();
    std::vector<char> seed = readSeed();
    int length = seed.size();
    for(unsigned int i=0;i<itr;i++)
    {
      std::cout<<std::endl;
      if(i>0)
      {
      if((i+1)%500==0)
      {
        seed = append_seed(i,itr,seed);
      }
      }
      mutate(seed,random,i);
    }
    
    
    return 0;
}
