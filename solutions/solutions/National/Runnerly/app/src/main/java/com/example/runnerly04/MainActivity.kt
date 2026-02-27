package com.example.runnerly04

import android.content.Intent
import android.content.res.Resources
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.content.res.ResourcesCompat
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.runnerly04.databinding.ActivityMainBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

const val BaseURL = "http://10.0.2.2:5000/api"
var id = 0

class MainActivity : AppCompatActivity() {
    lateinit var binding : ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.btnLogin.setOnClickListener {
            if (binding.etEmail.text.trim().isNullOrEmpty() ||
                binding.etPassword.text.trim().isNullOrEmpty()) {
                Toast.makeText(this@MainActivity, "Fill in all the data!", Toast.LENGTH_SHORT).show()
            } else {
                login()
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
            startActivity(
                Intent(this@MainActivity, RegisterActivity::class.java)
            )
        }
    }

    private fun login() {
        data class LoginBody (
            var email : String,
            var password : String,
        )

        var body = LoginBody (
            binding.etEmail.text.toString(),
            binding.etPassword.text.toString(),
        )

        var reqBody = buildReqBody(body)

        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Auth/login").openConnection() as HttpURLConnection

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
                        Toast.makeText(this@MainActivity, obj.getString("message"), Toast.LENGTH_SHORT).show()

                        var user = obj.getJSONObject("user")
                        id = user.getInt("id")

                        startActivity(
                            Intent(this@MainActivity, HomeActivity::class.java)
                        )
                    } else {
                        var obj = JSONObject(url.errorStream.bufferedReader().readText())
                        Toast.makeText(this@MainActivity, obj.getString("message"), Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }

    override fun onResume() {
        super.onResume()
        binding.etPassword.setText("")
        binding.etEmail.setText("")
    }
}

fun <T> buildReqBody(data : T) : String {
    var obj = JSONObject()
    var properties = data!!::class.java.declaredFields

    for (prop in properties) {
        prop.isAccessible = true
        obj.put(prop.name, prop.get(data))
    }

    return obj.toString()
}