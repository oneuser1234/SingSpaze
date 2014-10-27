using SingSpaze.Models;
using SingSpaze.Models.Input;
using SingSpaze.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SingSpaze.Controllers.API
{
    public class RecordingController : ApiController
    {
        private singspazeEntities db = new singspazeEntities();

        /// <summary>
        /// Add new record
        /// </summary>
        /// <param name="i_data">Class I_AddRecord</param>
        /// <returns>Class O_AddRecord</returns>
        [HttpPost]
        [ActionName("AddRecord")]
        public O_AddRecord AddRecord(I_AddRecord i_data)
        {
            if (i_data == null)
            {
                return new O_AddRecord()
                {
                    result = false,
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_AddRecord()
                {
                    result = false,
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }


            long user_id = Useful.getuserid(i_data.logindata.token);

            myrecord recorddata = new myrecord()
            {
                user_id = user_id,
                song_id = i_data.song_id,
                myrecord_description = i_data.description,
                myrecord_datetime = DateTime.Now,
                myrecord_length = i_data.length,
                myrecord_uploadURL = i_data.url
            };

            db.myrecord.Add(recorddata);
            db.SaveChanges();


            return new O_AddRecord()
            {
                result = true
            };
        }

        /// <summary>
        /// Edit record
        /// </summary>
        /// <param name="i_data">Class I_EditRecord</param>
        /// <returns>Class O_EditRecord</returns>
        [HttpPost]
        [ActionName("EditRecord")]
        public O_EditRecord EditRecord(I_EditRecord i_data)
        {
            if (i_data == null)
            {
                return new O_EditRecord()
                {
                    result = false,
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_EditRecord()
                {
                    result = false,
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }


            long user_id = Useful.getuserid(i_data.logindata.token);



            myrecord currecorddata = db.myrecord.FirstOrDefault(r => r.myrecord_id == i_data.record_id && r.user_id == user_id);
            if (currecorddata == null)
            {
                return new O_EditRecord()
                {
                    result = false,
                    errordata = Useful.geterror(6)
                };
            }


            currecorddata.user_id = user_id;
            currecorddata.song_id = i_data.song_id;
            currecorddata.myrecord_description = i_data.description;
            currecorddata.myrecord_datetime = DateTime.Now;
            currecorddata.myrecord_length = i_data.length;
            currecorddata.myrecord_uploadURL = i_data.url;
            db.SaveChanges();


            return new O_EditRecord()
            {
                result = true
            };
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="i_data">Class I_DeleteRecord</param>
        /// <returns>Class O_DeleteRecord</returns>
        [HttpPost]
        [ActionName("DeleteRecord")]
        public O_DeleteRecord DeleteRecord(I_DeleteRecord i_data)
        {
            if (i_data == null)
            {
                return new O_DeleteRecord()
                {
                    result = false,
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_DeleteRecord()
                {
                    result = false,
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }


            long user_id = Useful.getuserid(i_data.logindata.token);



            myrecord currecorddata = db.myrecord.FirstOrDefault(r => r.myrecord_id == i_data.record_id && r.user_id == user_id);
            if (currecorddata == null)
            {
                return new O_DeleteRecord()
                {
                    result = false,
                    errordata = Useful.geterror(6)
                };
            }


            db.myrecord.Remove(currecorddata);
            db.SaveChanges();


            return new O_DeleteRecord()
            {
                result = true
            };
        }

        /// <summary>
        /// Get Record
        /// </summary>
        /// <param name="i_data">Class I_GetRecord</param>
        /// <returns>Class O_GetRecord</returns>
        [HttpPost]
        [ActionName("GetRecord")]
        public O_GetRecord GetRecord(I_GetRecord i_data)
        {
            if (i_data == null)
            {
                return new O_GetRecord()
                {
                    errordata = Useful.geterror(11)
                };
            }

            if (Useful.checklogin(i_data.logindata) != null)
            {
                return new O_GetRecord()
                {
                    errordata = Useful.checklogin(i_data.logindata)
                };
            }


            long user_id = Useful.getuserid(i_data.logindata.token);

            List<myrecord> recorddata = db.myrecord.Where(r => r.user_id == user_id).OrderByDescending(r => r.myrecord_datetime).ToList();
            if (recorddata == null)
            {
                return new O_GetRecord()
                {
                    errordata = Useful.geterror(6)
                };
            }
            else
            {
                int resultnumber = recorddata.Count();
                List<Recorddata> o_record = new List<Recorddata>();
                foreach (myrecord data in recorddata)
                {
                    o_record.Add(new Recorddata()
                    {
                        id = data.myrecord_id,
                        songdata = Useful.getsongdata(data.song_id),
                        description = data.myrecord_description,
                        length = data.myrecord_length,
                        recordtime = data.myrecord_datetime,
                        url = data.myrecord_uploadURL
                    });
                }

                return new O_GetRecord()
                {
                    resultnumber = resultnumber,
                    recorddata = o_record
                };
            }





        }

    }
}
