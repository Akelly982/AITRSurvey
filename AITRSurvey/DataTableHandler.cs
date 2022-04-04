using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;            // needed for dataTable class

namespace AITRSurvey
{
    public class DataTableHandler
    {

        DataTable dt;

        public DataTableHandler(DataTable dt)
        {
            this.dt = dt;
        }


        // by removal of rows 
        // this is simpler than trying to generate a ne dt based of off the held data
        public DataTable getRowsByColumnNameAndIntValue(string columnName, int value)
        {

            //DataTable tempDt = this.dt.Copy(); 

            //V1
            // I am getting an error where when i remove the row from tempDt within
            // the loop it affects to collection our main dt as it states the collection has been modified
            //foreach (DataRow row in this.dt.Rows)
            //{
            //    if ((int)row[columnName] != value)
            //    {
            //        tempDt.Rows.Remove(row);
            //    }
            //}

            //V2
            // error here is that the index changes as you remove rows from the Data table
            ////hold a list of rows to remove
            //List<int> rowsToRemove = new List<int>(); 
            ////get the rows that need to be removed
            //for(int i = 0; i<this.dt.Rows.Count; i++)
            //{
            //    if ( (int)this.dt.Rows[i][columnName] != value){
            //        rowsToRemove.Add(i);
            //    }
            //}
            ////remove rows from temp data table
            //foreach( int x in rowsToRemove)
            //{
            //    tempDt.Rows.Remove(tempDt.Rows[x]);
            //}


            //V3
                //copy data table and clear all rows
                // we should have a dt with the same column names but empty
            DataTable tempDt = this.dt.Copy();
            tempDt.Rows.Clear();


            DataRow currentRow;
            foreach (DataRow row in this.dt.Rows)   //foreach row of the host
            {
                if ((int)row[columnName] == value)  // find the ones with our value
                {
                    // A row item array is a mix of String and Int32 datatypes hence not a for loop
                    // copy row
                    currentRow = tempDt.NewRow();
                    for (int i = 0; i < row.ItemArray.Length; i++)     
                    {
                        currentRow[i] = row[i];
                    }

                    //append row to tempDt
                    tempDt.Rows.Add(currentRow);
                    
                            
                }
            }


            return tempDt;
        }




    }


}