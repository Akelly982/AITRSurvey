using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;            // needed for dataTable class
using System.Web.UI.WebControls;

namespace AITRSurvey
{

    // manages data tables regardless of the number of columns or rows
    // allows basic querying returning multiple rows as a seperate dt
    // or a single data row

    // Additionally added functionality to get my ListItemCollections for Radio buttons and Check Boxes
    
    public class DataTableHandler
    {

        DataTable dt;

        public DataTableHandler(DataTable dt)
        {
            this.dt = dt;
        }


        public DataTable Dt { get => dt; set => dt = value; }


        // -------------------------
        // ------ Methods ----------
        // -------------------------

        // 
        //

        /// <summary>
        ///     get a dataTable from our this.dataTable 
        ///     for example where columnName == "QID_FK" and value = "200"
        ///     I did this because querying the database every time I needed data was to slow
        /// </summary>
        /// <param name="columnName"> the column name we are looking at within the datatable</param>
        /// <param name="value"> the value we are looking for within the data table column</param>
        /// <returns></returns>
        public DataTable getDataTableByColumnNameAndIntValue(string columnName, int value)
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
            DataTable tempDt = this.dt.Copy();   //saying dtable = other datatable does not seem to disconnect them may be used like a pointer
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

                    //append copied row to tempDt
                    tempDt.Rows.Add(currentRow);
                    
                }
            }


            return tempDt;
        }



        /// <summary>
        ///     get a singluar Data Row from this.DataTable 
        ///     for example where columnName == "QID" and value == "100" 
        /// </summary>
        /// <param name="columnName"> the column we are looking at within the dataTable </param>
        /// <param name="value"> the value we are looking for within the data table column </param>
        /// <returns></returns>
        public DataRow getRowByColumnNameAndIntValue(string columnName, int value)
        {

            DataRow tempRow = this.dt.NewRow();

            foreach (DataRow row in this.dt.Rows)   //foreach row of the host
            {
                if ((int)row[columnName] == value)  // find the ones with our value
                {
                    // A row item array is a mix of String and Int32 datatypes hence not a for loop
                    // copy row
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        tempRow[i] = row[i];
                    }

                }
            }

            //return row
            return tempRow;
        }




        //WARN Note: this will not work for datatable other than questionValues due to preset column names
        /// <summary>
        ///     gets a datatable from this.datatable based on your column and value selections 
        ///     then it converts the datatable to a list item collection using the preset
        ///     column names "text" and "NextQID" from questionValues
        ///     
        /// </summary>
        /// <param name="columnName">the column we are looking at within the datatable </param>
        /// <param name="value">the value we are looking for within the given data table column</param>
        /// <returns></returns>
        public ListItemCollection getListItemCollectionByColumnNameAndIntValue(string columnName, int value)
        {
            // create our new lic to fill with data
            ListItemCollection lic = new ListItemCollection();

            //using our other method grab the dt set we need 
            DataTable tempDt = getDataTableByColumnNameAndIntValue(columnName, value);

            // loop through dt getting our listItems
            foreach (DataRow row in tempDt.Rows)
            {
                ListItem li = new ListItem();
                li.Text = (string)row["text"];
                li.Value = ((int)row["NextQID"]).ToString();    // value has to be in the form of a String

                lic.Add(li);
            }

            return lic;
        }




    }


}