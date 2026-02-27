package com.example.mbcaandroid

import android.graphics.BitmapFactory
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.core.widget.addTextChangedListener
import com.example.mbcaandroid.databinding.ActivityEventDetailBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class EventDetailActivity : AppCompatActivity() {
    lateinit var binding : ActivityEventDetailBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityEventDetailBinding.inflate(layoutInflater)
        setContentView(binding.root)

        var objstring = intent.getStringExtra("obj")
        var obj = JSONObject(objstring)

        binding.toolbar.setNavigationOnClickListener {
            finish()
        }

        binding.btnTicket.setOnClickListener {
            if (binding.etQty.length() == 0) {
                Toast.makeText(this@EventDetailActivity, "Fill in all the data!", Toast.LENGTH_SHORT).show()
            } else if (binding.etQty.text.toString().toInt() <= 0) {
                Toast.makeText(this@EventDetailActivity, "Invalid Qty!", Toast.LENGTH_SHORT).show()
            } else {
                buy()
            }
        }

        binding.etQty.addTextChangedListener {
            if (binding.etQty.text.length > 0) {
                binding.btnTicket.text = "Buy ($" + binding.etQty.text.toString().toInt() * obj.getDouble("price")+ ")"
            } else {
                binding.btnTicket.text = "Buy"
            }
        }

        binding.tvName.text = obj.getString("title")
        binding.tvPrice.text = "Price: $" + obj.getDouble("price").toString() + "/person"
        binding.tvDate.text = "DateTime :" +  obj.getString("date")

        GlobalScope.launch(Dispatchers.IO) {
            var stream = URL("http://10.0.2.2:5000/Image/" + obj.getString("banner")).openConnection() as HttpURLConnection

            if (stream.responseCode == 200) {

                var bmp = BitmapFactory.decodeStream(stream.inputStream)

                withContext(Dispatchers.Main) {

                    bmp.let {
                        if (bmp != null) {
                            binding.iv.setImageBitmap(it)
                        }
                    }
                }
            }
        }
    }

    private fun buy() {
        var objstring = intent.getStringExtra("obj")
        var obj = JSONObject(objstring)

        data class BuyBody (
            var eventName : String,
            var qty : Int,
            var promoCode : String,
            var total : Double,
        )

        var body = BuyBody (
            binding.tvName.text.toString(),
            binding.etQty.text.toString().toInt(),
            binding.etPromo.text.toString(),
            binding.etQty.text.toString().toInt() * obj.getDouble("price")
        )

        var reqBody = buildReqBody(body)

        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Ticket").openConnection() as HttpURLConnection

            url.doInput = true
            url.doOutput = true
            url.requestMethod = "POST"
            url.setRequestProperty("Content-Type", "application/json")
            url.setRequestProperty("Authorization", "Bearer $token")

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
                        Toast.makeText(this@EventDetailActivity, "Success", Toast.LENGTH_SHORT).show()
                        finish()
                    } else {
                        Toast.makeText(this@EventDetailActivity, url.errorStream.bufferedReader().readText(), Toast.LENGTH_SHORT).show()
                    }
                }
            }
        }
    }
}