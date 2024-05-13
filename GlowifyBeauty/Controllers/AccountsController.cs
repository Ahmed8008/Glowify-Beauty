using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlowifyBeauty.Models;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using BCrypt.Net;
namespace GlowifyBeauty.Controllers
{
    public class AccountsController : Controller
    {
        //
        // GET: /Accounts/

        DataBase db = new DataBase();

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Accounts a, string newPassword, string ConfirmPassword)
        {




            db.con.Open();
            string checkEmailQuery = "SELECT COUNT(*) FROM [Accounts] WHERE Email = @Email";
            SqlCommand checkEmailCmd = new SqlCommand(checkEmailQuery, db.con);
            checkEmailCmd.Parameters.AddWithValue("@Email", a.Email);
            int existingUserCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

            if (existingUserCount > 0)
            {
                ViewBag.ErrorMessage = "this email already exists";
                return View();
            }
            else
            {
                if (newPassword != ConfirmPassword)
                {
                    ViewBag.Result2 = "The password does not match the confirm password.";
                    return View();
                }


                if (!IsPasswordValid(newPassword))
                {
                    db.con.Close();
                    ViewBag.Result2 = "Your password must be at least 8 characters long and include at least one uppercase letter and one special character.";
                    return View(); // Show ResetPassword view again with the error message
                }
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                string q = "insert into [Accounts] (FirstName,LastName,ContactNumber,Email,Password,ResetPasswordToken,ResetPasswordTokenExpiry,roles) values(@FirstName,@LastName,@ContactNumber,@Email,@Password,@ResetPasswordToken,NULL,@roles)";
                SqlCommand cmd = new SqlCommand(q, db.con);
                cmd.Parameters.AddWithValue("@FirstName", a.FirstName);
                cmd.Parameters.AddWithValue("@LastName", a.LastName);
                cmd.Parameters.AddWithValue("@ContactNumber", a.ContactNumber);
                cmd.Parameters.AddWithValue("@Email", a.Email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@ResetPasswordToken", "None");
                cmd.Parameters.AddWithValue("@roles", "User");
                cmd.ExecuteNonQuery();
                db.con.Close();
                db.con.Open();
                ViewBag.Result = "Account Created Successfully";
                return View();
            }

        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(Accounts a)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists with the provided email
                db.con.Open();
                string query = "SELECT * FROM Accounts WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, db.con);
                cmd.Parameters.AddWithValue("@Email", a.Email);

                SqlDataReader reader = cmd.ExecuteReader();

                Accounts user = null;
                if (reader.Read())
                {
                    user = new Accounts
                    {
                        Account_id = Convert.ToInt32(reader["Account_id"])
                        // Assuming other properties are not needed here for generating the link
                    };
                }

                reader.Close();

                if (user != null)
                {
                    // Generate a unique token for password reset
                    string token = Guid.NewGuid().ToString();

                    // Update the user's reset token and its expiry time in the database
                    string updateQuery = "UPDATE Accounts SET ResetPasswordToken = @Token, ResetPasswordTokenExpiry = @Expiry WHERE Email = @Email";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, db.con);
                    updateCmd.Parameters.AddWithValue("@Token", token);
                    updateCmd.Parameters.AddWithValue("@Expiry", DateTime.Now.AddHours(1));
                    updateCmd.Parameters.AddWithValue("@Email", a.Email);

                    updateCmd.ExecuteNonQuery();

                    // Send email with the password reset link
                    var senderemail = new MailAddress("demo7899999@gmail.com", "Password Reset");
                    var password = "bfapmnypsntylcde";

                    var receiveremail = new MailAddress(a.Email, "Receiver");

                    // Build the email body with the password reset link
                    var callbackUrl = Url.Action("ResetPassword", "Accounts", new { AccountID = user.Account_id, token = token }, Request.Url.Scheme);
                    string body = "Please reset your password by clicking <a href='" + callbackUrl + "'>here</a>";

                    // Send the email
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderemail.Address, password)
                    };

                    using (var mess = new MailMessage(senderemail, receiveremail))
                    {
                        mess.Subject = "Password Reset";
                        mess.Body = body;
                        mess.IsBodyHtml = true;

                        smtp.Send(mess);
                    }

                    ViewBag.Result = "Password reset instructions have been sent to your email.";
                }
                else
                {
                    ViewBag.Result = "No user found with this email.";
                }

                db.con.Close();
            }

            return View();
        }

        public ActionResult ResetPassword(string AccountID, string token)
        {
            db.con.Open();
            string query = "SELECT * FROM Accounts WHERE Account_id = @Account_id AND ResetPasswordToken = @Token AND ResetPasswordTokenExpiry > @CurrentTime";
            SqlCommand cmd = new SqlCommand(query, db.con);
            cmd.Parameters.AddWithValue("@Account_id", AccountID);
            cmd.Parameters.AddWithValue("@Token", token);
            cmd.Parameters.AddWithValue("@CurrentTime", DateTime.Now);

            SqlDataReader reader = cmd.ExecuteReader();
            Accounts user = null;

            if (reader.Read())
            {
                user = new Accounts
                {
                    // Populate user properties as needed
                    // For example: Id = Convert.ToInt32(reader["User_Id"]),
                };
            }

            reader.Close();
            db.con.Close();

            if (user == null)
            {
                // Token is invalid or expired, handle accordingly (e.g., show an error message)
                ViewBag.Result = "Invalid or expired token.";
                return View(); // You need to create an Error view with appropriate content
            }

            // If the token is valid, allow the user to reset their password
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string AccountID, string token, string newPassword, string ConfirmPassword)
        {
            // Check if the token is valid and get the user details
            db.con.Open();
            string query = "SELECT * FROM Accounts WHERE Account_id = @Account_id AND ResetPasswordToken = @Token AND ResetPasswordTokenExpiry > @CurrentTime";
            SqlCommand cmd = new SqlCommand(query, db.con);
            cmd.Parameters.AddWithValue("@Account_id", AccountID);
            cmd.Parameters.AddWithValue("@Token", token);
            cmd.Parameters.AddWithValue("@CurrentTime", DateTime.Now);

            SqlDataReader reader = cmd.ExecuteReader();
            Accounts user = null;

            if (reader.Read())
            {
                user = new Accounts
                {
                    // Populate user properties as needed
                    // For example: Id = Convert.ToInt32(reader["User_Id"]),
                };
            }

            reader.Close();



            if (user == null)
            {
                db.con.Close();
                // Token is invalid or expired, handle accordingly (e.g., show an error message)
                ViewBag.Result = "Invalid or expired token.";
                return View(); // You need to create an Error view with appropriate content
            }

            if (newPassword != ConfirmPassword)
            {
                ViewBag.Result2 = "The password does not match the confirm password.";
                return View();
            }


            if (!IsPasswordValid(newPassword))
            {
                db.con.Close();
                ViewBag.Result2 = "Your password must be at least 8 characters long and include at least one uppercase letter and one special character.";
                return View(); // Show ResetPassword view again with the error message
            }


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Reset the user's password
            string updateQuery = "UPDATE Accounts SET Password = @Password, ResetPasswordToken = NULL, ResetPasswordTokenExpiry = NULL WHERE Account_id = @Account_id";
            SqlCommand updateCmd = new SqlCommand(updateQuery, db.con);
            updateCmd.Parameters.AddWithValue("@Password", hashedPassword);
            updateCmd.Parameters.AddWithValue("@Account_id", AccountID);
            updateCmd.ExecuteNonQuery();

            db.con.Close();

            ViewBag.Result = "Password has been reset successfully.";
            return View(); // Redirect the user to the login page or any other appropriate page
        }


        private bool IsPasswordValid(string password)
        {
            // Minimum length check
            if (password.Length < 8)
                return false;

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
                return false;

            // Check for at least one special character
            if (!password.Any(IsSpecialCharacter))
                return false;

            return true;
        }

        private bool IsSpecialCharacter(char c)
        {
            // Define your set of special characters here
            string specialCharacters = "!@#$%^&*()-_=+[]{}|;:'\",.<>/?";

            // Check if the character is a special character
            return specialCharacters.Contains(c);
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }



        [HttpPost]
        public ActionResult SignIn(Accounts a)
        {
            db.con.Open();
            string q = "SELECT * FROM [Accounts] WHERE Email=@Email";
            SqlCommand cmd = new SqlCommand(q, db.con);
            cmd.Parameters.AddWithValue("@Email", a.Email);

            SqlDataReader sdr = cmd.ExecuteReader();

            if (sdr.Read())
            {
                string storedHashedPassword = sdr["Password"].ToString();

                if (BCrypt.Net.BCrypt.Verify(a.Password, storedHashedPassword))
                {
                    // Passwords match, sign in the user
                    string role = sdr["roles"].ToString();
                    Session["roles"] = role;
                    Session["Email"] = a.Email;
                    Session["Account_id"] = sdr["Account_id"].ToString();
                    Session["FirstName"] = sdr["FirstName"].ToString();
                    Session["LastName"] = sdr["LastName"].ToString();
                    sdr.Close();
                    db.con.Close();

                    // Determine the role and redirect accordingly
                    if (role == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "Admin");
                    }

                    else if (role == "User")
                    {
                        return RedirectToAction("Homepage", "Home");
                    }
                    // Add more roles as needed
                }
            }

            // Incorrect email or password
            ViewBag.Result = "Email or Password is incorrect";
            sdr.Close();
            db.con.Close();
            return View();
        }



        public ActionResult LogOut()
        {
            Session.RemoveAll(); //Clear all session variables
            return RedirectToAction("HomePage", "Home");
        }

    }
}
