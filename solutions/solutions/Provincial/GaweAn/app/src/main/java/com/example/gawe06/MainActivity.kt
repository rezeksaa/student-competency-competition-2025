package com.example.gawe06

import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.gawe06.databinding.ActivityMainBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class MainActivity : AppCompatActivity() {
    lateinit var binding : ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.tvRegister.setOnClickListener {
            startActivity(
                Intent(this@MainActivity, RegisterActivity::class.java)
            )
        }

        binding.btnLogin.setOnClickListener {
            if (binding.etEmail.text.trim().toString().isNullOrEmpty() ||binding.etPassword.text.trim().toString().isNullOrEmpty() ) {
                Toast.makeText(this@MainActivity, "Fill in all the data!", Toast.LENGTH_SHORT).show()
            } else {
                data class UserModel (
                    var email : String,
                    var password : String,
                )

                var userModel = UserModel(
                    binding.etEmail.text.toString(),
                    binding.etPassword.text.toString(),
                )

                var reqBody = buildReqBody(userModel)
                Login(reqBody)
            }
        }
    }

    private fun Login(reqBody: String) {
        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/auth").openConnection() as HttpURLConnection

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

                        var user = JSONObject(res)
                        var data = user.getJSONObject("data")

                        if (data.getString("role") == "JobSeeker") {
                            startActivity(Intent(this@MainActivity, AllActivity::class.java))
                        } else {
                            Toast.makeText(this@MainActivity, "Only job seeker that can login to the system.", Toast.LENGTH_SHORT).show()
                        }

                    } else {
                        Toast.makeText(this@MainActivity, "Email or password is incorrect!", Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }

    override fun onResume() {
        super.onResume()
        binding.apply {
            etEmail.setText("")
            etPassword.setText("")
        }
    }
}