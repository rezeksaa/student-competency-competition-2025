package com.example.runnerly04

import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.runnerly04.databinding.ActivityRegisterBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class RegisterActivity : AppCompatActivity() {
    lateinit var binding : ActivityRegisterBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRegisterBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.btnLogin.setOnClickListener {
            if (binding.etEmail.text.trim().isNullOrEmpty() ||
                binding.etPassword.text.trim().isNullOrEmpty()||
                binding.etname.text.trim().isNullOrEmpty()||
                binding.etAge.text.trim().isNullOrEmpty()) {
                Toast.makeText(this@RegisterActivity, "Fill in all the data!", Toast.LENGTH_SHORT).show()
            } else if (binding.etAge.text.toString().toInt() < 13){
                Toast.makeText(this@RegisterActivity, "You have to be minimum 13 years old to use this app", Toast.LENGTH_SHORT).show()
            } else if (binding.etPassword.text.length < 6) {
                Toast.makeText(this@RegisterActivity, "the minimum character for a password is 6 character", Toast.LENGTH_SHORT).show()
            } else {
                register()
            }
        }

        binding.look.setOnClickListener {
            if (binding.etPassword.inputType == 129) {
                binding.etPassword.inputType = 1
            } else {
                binding.etPassword.inputType = 129
            }
        }

        binding.tvLink.setOnClickListener {
            finish()
        }
    }

    private fun register() {
        data class LoginBody (
            var email : String,
            var password : String,
            var fullName : String,
            var age : Int
        )

        var body = LoginBody (
            binding.etEmail.text.toString(),
            binding.etPassword.text.toString(),
            binding.etname.text.toString(),
            binding.etAge.text.toString().toInt()
        )

        var reqBody = buildReqBody(body)

        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Auth/register").openConnection() as HttpURLConnection

            url.doInput = true
            url.doOutput = true

            url.requestMethod = "POST"
            url.setRequestProperty("Content-Type", "application/json")
            url.outputStream.writer().apply {
                write(reqBody)
                flush()
                close()
            }

            var code = url.responseCode
            var res = if (code in 200 .. 299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var obj = JSONObject(res)
                        Toast.makeText(this@RegisterActivity, obj.getString("message" )+ ", Please login", Toast.LENGTH_SHORT).show()

                        finish()
                    } else {
                        var obj = JSONObject(url.errorStream.bufferedReader().readText())
                        Toast.makeText(this@RegisterActivity, obj.getString("message"), Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }
}