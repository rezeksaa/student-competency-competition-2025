package com.example.mbcaandroid

import android.os.Bundle
import android.util.Patterns
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.mbcaandroid.databinding.ActivityRegisterBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONArray
import java.net.HttpURLConnection
import java.net.URL
import java.util.regex.Pattern

class RegisterActivity : AppCompatActivity() {
    lateinit var binding : ActivityRegisterBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityRegisterBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.btnLogin.setOnClickListener {
            if (binding.etEmail.text.trim().isNullOrEmpty() ||
                binding.etPassword.text.trim().isNullOrEmpty() ||
                binding.etUsername.text.trim().isNullOrEmpty() ||
                binding.etPhoneNumber.text.trim().isNullOrEmpty() ||
                binding.etFullName.text.trim().isNullOrEmpty() ) {
                Toast.makeText(this@RegisterActivity, "Fill in all the data!", Toast.LENGTH_SHORT).show()
            } else if (!Patterns.EMAIL_ADDRESS.matcher(binding.etEmail.text).matches()){
                Toast.makeText(this@RegisterActivity, "Email is not in valid format!", Toast.LENGTH_SHORT).show()
            } else if (binding.etPassword.text.length < 8) {
                Toast.makeText(this@RegisterActivity, "The minimum haracter for a password is 8!", Toast.LENGTH_SHORT).show()
            } else {
                register()
            }
        }

        binding.tvLink.setOnClickListener {
            finish()
        }

        getPrefix()
    }

    private fun register() {
        data class RegisterBody (
            var username:String,
            var fullName:String,
            var email:String,
            var phoneNumber:String,
            var password:String,
        )

        var body = RegisterBody(
            binding.etUsername.text.toString(),
            binding.etFullName.text.toString(),
            binding.etEmail.text.toString(),
            binding.spinnerPrefix.selectedItem.toString()+binding.etPhoneNumber.text.toString(),
            binding.etPassword.text.toString(),
        )

        var reqBody = buildReqBody(body)

        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Auth/Register").openConnection() as HttpURLConnection

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
                res.let{
                    if (res != null) {
                        Toast.makeText(this@RegisterActivity, "Success, please login", Toast.LENGTH_SHORT).show()
                        finish()
                    } else {
                        Toast.makeText(this@RegisterActivity, url.errorStream.bufferedReader().readText(), Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }

    private fun getPrefix() {
        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Prefix").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200 .. 299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var arr = JSONArray(res)
                        var prefix = mutableListOf<String>()
                        for (i in 0 until arr.length()) {
                            prefix.add(arr.getString(i))
                        }

                        binding.spinnerPrefix.adapter = ArrayAdapter(this@RegisterActivity, android.R.layout.simple_spinner_item, prefix)
                    }
                }
            }
        }
    }
}