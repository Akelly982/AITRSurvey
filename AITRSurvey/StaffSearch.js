
//Document ready seems to not work.
//$(document).ready(function () {
//    alert("js loaded.");
//})

//alert("hello i am js");

//document.getElementById("idListHolder").value = "hello i am value";
//var idListCsv = document.getElementById("idListHolder").value;

//test values output
console.log(document.getElementById("idListHolder").value);
console.log(document.getElementById("itemTypeListHolder").value)

//get csv strings
var idCsv = document.getElementById("idListHolder").value
var itemTypeCsv = document.getElementById("itemTypeListHolder").value

//convert csv to values
var idArr = idCsv.split(",");
var itemTypeArr = itemTypeCsv.split(",");

//ensure bot arrays are the same length
if (idArr.length == itemTypeArr.length) {
    console.log("Array length matches");

    //for each item id get user selection
    for (let i = 0; i < idArr.length; i++) {

        //get current question element 
        var currentElement = document.getElementById(idArr[i]);

        //determin item type 
        if (itemTypeArr[i] == 1) {  //textbox
            
        }
        else{  //check box or radio btn  <-- both are set up as check boxes here

            //find what the user has selsected

        }
    }

} else {
    //array length does not match
    alert("Error id and item type array mismatch.");
}



