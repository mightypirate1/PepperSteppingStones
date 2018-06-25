using UnityEditor;
using UnityEngine;
using System.Collections;

public class BuildScript: MonoBehaviour
{
      static string reward = "rDirect";
      // static string reward = "rLinear";
      // static string reward = "rConstant";

      static void BuildAll()
     {
         BuildBig();
         BuildSimplified();
         BuildFirst();
     }

     static void BuildBig()
     {
       string[] scenes = {
         "Assets/Scenarios/PepperSocial/PepperSocialBig.unity",
       };
       string target = "build/big_" +reward+ ".x86_64";
       BuildPipeline.BuildPlayer(scenes, target, BuildTarget.StandaloneLinux64, BuildOptions.None);
     }
     static void BuildSimplified()
     {
       string[] scenes = {
         "Assets/Scenarios/PepperSocial/PepperSocialSimplified.unity",
       };
       string target = "build/simplified_" +reward+ ".x86_64";
       BuildPipeline.BuildPlayer(scenes, target, BuildTarget.StandaloneLinux64, BuildOptions.None);

     }
     static void BuildFirst()
     {
       string[] scenes = {
         "Assets/Scenarios/PepperSocial/PepperSocial_1st.unity",
       };
       string target = "build/first_" +reward+ ".x86_64";
       BuildPipeline.BuildPlayer(scenes, target, BuildTarget.StandaloneLinux64, BuildOptions.None);

     }

}
