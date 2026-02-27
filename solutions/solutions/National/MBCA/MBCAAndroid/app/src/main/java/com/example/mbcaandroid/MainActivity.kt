package com.example.mbcaandroid

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.mbcaandroid.databinding.ActivityMainBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import kotlin.math.log

const val BaseURL = "http://10.0.2.2:5000/api"
var token = ""

class MainActivity : AppCompatActivity() {
    lateinit var binding : ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding= ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.btnLogin.setOnClickListener {
            if (binding.etPassword.text.trim().isNullOrEmpty() ||
                binding.etUsername.text.trim().isNullOrEmpty()) {
                Toast.makeText(this@MainActivity, "Fill in all the data!", Toast.LENGTH_SHORT).show()
            } else {
                login()
            }
        }

        binding.tvLink.setOnClickListener {
            startActivity(
                Intent(this@MainActivity, RegisterActivity::class.java)
            )
        }
    }

    override fun onResume() {
        super.onResume()
        binding.etPassword.setText("")
        binding.etUsername.setText("")
    }

    private fun login() {

        data class LoginBody (
            var username : String,
            var password : String,
        )

        var body = LoginBody (
            binding.etUsername.text.toString(),
            binding.etPassword.text.toString(),
        )

        var reqBody = buildReqBody(body)

        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Auth/Login").openConnection() as HttpURLConnection

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
                        token = res.toString()

                        startActivity(Intent(this@MainActivity, HomeActivity::class.java))
                    } else {
                        Toast.makeText(this@MainActivity, url.errorStream.bufferedReader().readText(), Toast.LENGTH_SHORT).show()
                    }
                }
            }

        }
    }
}

fun <T> buildReqBody(data : T) :String {
    var obj = JSONObject()
    var properties = data!!::class.java.declaredFields

    for (prop in properties ) {
        prop.isAccessible = true
        obj.put(prop.name, prop.get(data))
    }

    return  obj.toString()
}