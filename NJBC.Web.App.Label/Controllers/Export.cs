using NJBC.DataLayer.Models.Semeval2015;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBC.Web.App.Label.Controllers
{
    public class Export
    {
        string SemEvalTrain(List<Question> questions)
        {
//            string A = @"<?xml version="1.0" encoding="utf-8"?>
//< !DOCTYPE xml[
// < !ELEMENT xml(OrgQuestion *) >
// < !ATTLIST xml
// version CDATA #REQUIRED
//>
// < !ELEMENT OrgQuestion(OrgQSubject, OrgQBody, Thread) >
// < !ATTLIST OrgQuestion
// ORGQ_ID CDATA #REQUIRED
//>
// < !ELEMENT OrgQSubject(#PCDATA)>
//< !ELEMENT OrgQBody(#PCDATA)>
//< !ELEMENT Thread(RelQuestion, RelComment *) >
// < !ATTLIST Thread
 
//  THREAD_SEQUENCE CDATA #REQUIRED
// SubtaskA_Skip_Because_Same_As_RelQuestion_ID CDATA #IMPLIED
//>
// < !ELEMENT RelQuestion(RelQSubject, RelQBody) >
// < !ATTLIST RelQuestion
// RELQ_ID CDATA #REQUIRED
//RELQ_RANKING_ORDER CDATA #REQUIRED
//RELQ_CATEGORY CDATA #REQUIRED
//RELQ_DATE CDATA #REQUIRED
//RELQ_USERID CDATA #REQUIRED
//RELQ_USERNAME CDATA #REQUIRED
//RELQ_RELEVANCE2ORGQ CDATA #REQUIRED
//>
// < !ELEMENT RelQSubject(#PCDATA)>
//< !ELEMENT RelQBody(#PCDATA)>
//< !ELEMENT RelComment(RelCText) >
// < !ATTLIST RelComment
// RELC_ID CDATA #REQUIRED
//RELC_DATE CDATA #REQUIRED
//RELC_USERID CDATA #REQUIRED
//RELC_USERNAME CDATA #REQUIRED
//RELC_RELEVANCE2ORGQ CDATA #REQUIRED
//RELC_RELEVANCE2RELQ CDATA #REQUIRED
//>
// < !ELEMENT RelCText(#PCDATA)>
//]>
// < xml version = "1.0" >
 

// < OrgQuestion ORGQ_ID = "Q1" >
 
//     < OrgQSubject > Massage oil </ OrgQSubject >
 
//     < OrgQBody > Where I can buy good oil for massage ?</ OrgQBody >
    

//        < Thread THREAD_SEQUENCE = "Q1_R1" >
    
//            < RelQuestion RELQ_ID = "Q1_R1" RELQ_RANKING_ORDER = "1" RELQ_CATEGORY = "Qatar Living Lounge" RELQ_DATE = "2010-08-27 01:38:59" RELQ_USERID = "U1" RELQ_USERNAME = "sognabodl" RELQ_RELEVANCE2ORGQ = "PerfectMatch" >
    
//                < RelQSubject > massage oil </ RelQSubject >
    
//                < RelQBody >is there any place i can find scented massage oils in qatar ?</ RelQBody >
    
//            </ RelQuestion >
    

//            < RelComment RELC_ID = "Q1_R1_C1" RELC_DATE = "2010-08-27 01:40:05" RELC_USERID = "U2" RELC_USERNAME = "anonymous" RELC_RELEVANCE2ORGQ = "Good" RELC_RELEVANCE2RELQ = "Good" >
    
//                < RelCText > Yes.It is right behind Kahrama in the National area.</ RelCText >
    
//            </ RelComment >
    

//            < RelComment RELC_ID = "Q1_R1_C2" RELC_DATE = "2010-08-27 01:42:59" RELC_USERID = "U1" RELC_USERNAME = "sognabodl" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Bad" >
    
//                < RelCText > whats the name of the shop ?</ RelCText >
    
//            </ RelComment >
    

//            < RelComment RELC_ID = "Q1_R1_C3" RELC_DATE = "2010-08-27 01:44:09" RELC_USERID = "U2" RELC_USERNAME = "anonymous" RELC_RELEVANCE2ORGQ = "Good" RELC_RELEVANCE2RELQ = "Good" >
    
//                < RelCText > It's called Naseem Al-Nadir. Right next to the Smartlink shop. You'll find the chinese salesgirls at affordable prices there.</ RelCText >
    
//            </ RelComment >
    

//            < RelComment RELC_ID = "Q1_R1_C4" RELC_DATE = "2010-08-27 01:58:39" RELC_USERID = "U1" RELC_USERNAME = "sognabodl" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Bad" >
    
//                < RelCText > dont want girls; want oil </ RelCText >
    
//            </ RelComment >
    

//            < RelComment RELC_ID = "Q1_R1_C5" RELC_DATE = "2010-08-27 01:59:55" RELC_USERID = "U2" RELC_USERNAME = "anonymous" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Good" >
    
//                < RelCText > Try Both ;) I'am just trying to be helpful. On a serious note - Please go there. you'll find what you are looking for.</ RelCText >
       
//               </ RelComment >
       

//               < RelComment RELC_ID = "Q1_R1_C6" RELC_DATE = "2010-08-27 02:02:53" RELC_USERID = "U3" RELC_USERNAME = "lawa" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Bad" >
       
//                   < RelCText > you mean oil and filter both </ RelCText >
       
//               </ RelComment >
       

//               < RelComment RELC_ID = "Q1_R1_C7" RELC_DATE = "2010-08-27 02:04:29" RELC_USERID = "U2" RELC_USERNAME = "anonymous" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Bad" >
       
//                   < RelCText > Yes Lawa...you couldn't be more right LOL</RelCText>
//               </ RelComment >
       

//               < RelComment RELC_ID = "Q1_R1_C8" RELC_DATE = "2010-08-27 02:08:19" RELC_USERID = "U2" RELC_USERNAME = "anonymous" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Bad" >
       
//                   < RelCText > What they offer ?</ RelCText >
       
//               </ RelComment >
       

//               < RelComment RELC_ID = "Q1_R1_C9" RELC_DATE = "2010-08-27 02:08:39" RELC_USERID = "U4" RELC_USERNAME = "Swine Flu" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Bad" >
       
//                   < RelCText > FU did u try with that salesgirl ?</ RelCText >
          
//                  </ RelComment >
          

//                  < RelComment RELC_ID = "Q1_R1_C10" RELC_DATE = "2010-08-27 02:10:33" RELC_USERID = "U2" RELC_USERNAME = "anonymous" RELC_RELEVANCE2ORGQ = "Bad" RELC_RELEVANCE2RELQ = "Bad" >
                     
//                                 < RelCText > Swine - No I don't try with salesgirls. My taste is classy ;)</RelCText>
//                                </ RelComment >
                        
//                            </ Thread >
//                        </ OrgQuestion >
//                        </ xml > ';
            return "";
        }
    }
}
