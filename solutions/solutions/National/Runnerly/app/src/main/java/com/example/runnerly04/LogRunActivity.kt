package com.example.runnerly04

import android.app.DatePickerDialog
import android.content.Intent
import android.os.Bundle
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.runnerly04.databinding.ActivityLogRunBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class LogRunActivity : AppCompatActivity() {
    lateinit var binding: ActivityLogRunBinding
    var date = LocalDate.now()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityLogRunBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.tvDate.text = date.format(DateTimeFormatter.ofPattern("MMM dd, yyyy"))

        binding.tvDate.setOnClickListener {
            DatePickerDialog(this@LogRunActivity).apply {
                updateDate(date.year, date.monthValue - 1, date.dayOfMonth)
                setOnDateSetListener { view, year, month, dayOfMonth ->
                    date = LocalDate.of(year, month + 1, dayOfMonth)
                    if (date > LocalDate.now()) {
                        date = LocalDate.now()
                        Toast.makeText(
                            this@LogRunActivity,
                            "It's not possible to log your future run",
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                    binding.tvDate.text = date.format(DateTimeFormatter.ofPattern("MMM dd, yyyy"))
                }
            }.show()
        }

        binding.Back.setOnClickListener {
            finish()
        }

        binding.btnSave.setOnClickListener {
            if (
                binding.etDistance.text.trim().isNullOrEmpty()
            ) {
                Toast.makeText(this@LogRunActivity, "Fill in all the data", Toast.LENGTH_SHORT)
                    .show()
            } else if (binding.etHour.text.trim().isNullOrEmpty() &&
                binding.etSecond.text.trim().isNullOrEmpty() &&
                binding.etMinute.text.trim().isNullOrEmpty()
            ) {
                Toast.makeText(
                    this@LogRunActivity,
                    "Invalid time, minimum of ten minute!",
                    Toast.LENGTH_SHORT
                ).show()
            } else if (binding.etMinute.text.trim().toString().toInt() < 10) {
                Toast.makeText(
                    this@LogRunActivity,
                    "Invalid time, minimum of ten minute!",
                    Toast.LENGTH_SHORT
                ).show()
            } else if (binding.etDistance.text.toString().toDouble() <= 0) {
                Toast.makeText(this@LogRunActivity, "Invalid Distance", Toast.LENGTH_SHORT).show()
            } else {

                if (binding.etMinute.text.length != 0 && binding.etMinute.text.toString().toInt() >= 60) {
                    Toast.makeText(this@LogRunActivity, "Invalid Minute", Toast.LENGTH_SHORT).show()
                } else if (binding.etSecond.text.length != 0 && binding.etSecond.text.toString().toInt() >= 60) {
                    Toast.makeText(this@LogRunActivity, "Invalid Second", Toast.LENGTH_SHORT).show()
                } else {
                    postRun()
                }

            }
        }

    }

    private fun postRun() {
        data class Run(
            var userId: Int,
            var date: String,
            var distance: Double,
            var duration: JSONObject,
        )

        var addHour = if (binding.etHour.text.length != 2) "0" else ""
        var addMinute = if (binding.etMinute.text.length != 2) "0" else ""
        var addSecond = if (binding.etSecond.text.length != 2) "0" else ""
        addHour = if (binding.etHour.text.length == 0) "00" else addHour
        addMinute = if (binding.etMinute.text.length == 0) "00" else addMinute
        addSecond = if (binding.etSecond.text.length == 0) "00" else addSecond

        var time ="$addHour${binding.etHour.text}:$addMinute${binding.etMinute.text}:$addSecond${binding.etSecond.text}"

        var obj = JSONObject()
        obj.put("time", time)

        var run = Run(
            id,
            date.format(DateTimeFormatter.ofPattern("dd-MM-yyyy")),
            binding.etDistance.text.toString().toDouble(),
            obj
        )

        var reqBody = buildReqBody(run)

        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Activities").openConnection() as HttpURLConnection

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
            var res = if (code in 200..299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var obj = JSONObject(res)
                        Toast.makeText(
                            this@LogRunActivity,
                            "Success",
                            Toast.LENGTH_SHORT
                        ).show()

                        finish()
                    } else {
                        var obj = JSONObject(url.errorStream.bufferedReader().readText())
                        Toast.makeText(
                            this@LogRunActivity,
                            obj.getString("message"),
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                }
            }
        }
    }
}