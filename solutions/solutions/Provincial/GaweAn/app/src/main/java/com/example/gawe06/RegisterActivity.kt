package com.example.gawe06

import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.gawe06.databinding.ActivityRegisterBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

const val BaseURL = "http://10.0.2.2:5000/api"

class RegisterActivity : AppCompatActivity() {
    lateinit var binding : ActivityRegisterBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRegisterBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.tvLogin.setOnClickListener {
            finish()
        }

        binding.btnRegister.setOnClickListener {
            if (validasi()) {

                data class UserModel (
                    var fullname : String,
                    var email : String,
                    var phoneNumber : String,
                    var password : String,
                    var confirmPassword : String,
                )

                var userModel = UserModel(
                    binding.etFullName.text.toString(),
                    binding.etEmail.text.toString(),
                    binding.etPhoneNumber.text.toString(),
                    binding.etPassword.text.toString(),
                    binding.etConfirmPassword.text.toString(),
                )

                var reqBody = buildReqBody(userModel)

                Register(reqBody)

            }
        }
    }

    private fun Register(reqBody: String) {
        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/register").openConnection() as HttpURLConnection

            url.doInput = true
            url.doOutput = true

            url.requestMethod = "POST"
            url.setRequestProperty("Content-Type", "application/json")

            var outputstream = url.outputStream
            var writer = outputstream.writer()

            writer.apply {
                write(reqBody)
                flush()
                close()
            }

            var code = url.responseCode
            var res = if (code in 200 .. 299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {

                        Toast.makeText(this@RegisterActivity, "Register Successfull please login at the login page!", Toast.LENGTH_SHORT).show()
                        finish()

                    } else {
                        Toast.makeText(this@RegisterActivity, "Email is already used!", Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }

    fun validasi() : Boolean {
        if (binding.etPassword.text.trim().toString() != binding.etConfirmPassword.text.trim().toString()) {
            Toast.makeText(this@RegisterActivity, "Confirm password doesn't match", Toast.LENGTH_SHORT).show()
            return false
        }

        binding.apply {
            if (etPassword.text.trim().isNullOrEmpty() ||etPhoneNumber.text.trim().isNullOrEmpty() ||etConfirmPassword.text.trim().isNullOrEmpty() ||etEmail.text.trim().isNullOrEmpty() ||etFullName.text.trim().isNullOrEmpty()) {
                Toast.makeText(this@RegisterActivity, "Fill in all the data!", Toast.LENGTH_SHORT).show()
                return false
            }
        }

        return true
    }
}

fun <T> buildReqBody (data: T): String {
    var jsonObj = JSONObject()
    var properties = data!!::class.java.declaredFields

    for (prop in properties) {
        prop.isAccessible = true
        jsonObj.put(prop.name, prop.get(data))
    }

    return jsonObj.toString()
}