package com.example.runnerly04

import android.app.DatePickerDialog
import android.os.Bundle
import android.view.LayoutInflater
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.runnerly04.databinding.ActivityMyRunBinding
import com.example.runnerly04.databinding.ItemMyRunBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class MyRunActivity : AppCompatActivity() {
    lateinit var binding : ActivityMyRunBinding

    var startDate = LocalDate.now()
    var endDate = LocalDate.now().plusDays(1)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMyRunBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.toolbar.setNavigationOnClickListener {
            finish()
        }

        binding.tvStartDate.text = startDate.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))
        binding.tvEndDate.text = endDate.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))

        binding.tvStartDate.setOnClickListener {
            DatePickerDialog(this@MyRunActivity).apply {
                updateDate(startDate.year, startDate.monthValue - 1, startDate.dayOfMonth)
                setOnDateSetListener { view, year, month, dayOfMonth ->
                    startDate = LocalDate.of(year, month + 1, dayOfMonth)
                    if (startDate > endDate) {
                        startDate = endDate.minusDays(1)
                        Toast.makeText(this@MyRunActivity, "Invalid date!", Toast.LENGTH_SHORT).show()
                    }
                    binding.tvStartDate.text = startDate.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))
                    getData()
                }
            }.show()
        }

        binding.tvEndDate.setOnClickListener {
            DatePickerDialog(this@MyRunActivity).apply {
                updateDate(endDate.year, endDate.monthValue - 1, endDate.dayOfMonth)
                setOnDateSetListener { view, year, month, dayOfMonth ->
                    endDate = LocalDate.of(year, month + 1, dayOfMonth)
                    if (endDate < startDate) {
                        endDate = startDate.plusDays(1)
                        Toast.makeText(this@MyRunActivity, "Invalid date!", Toast.LENGTH_SHORT).show()
                    }
                    binding.tvEndDate.text = endDate.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))
                    getData()
                }
            }.show()
        }

        getData()
    }

    private fun getData() {
        data class Filter (
            var userId : Int,
            var startDate : String,
            var endDate : String,
        )

        var f = Filter (
            id,
            startDate.format(DateTimeFormatter.ofPattern("dd-MM-yyyy")),
            endDate.format(DateTimeFormatter.ofPattern("dd-MM-yyyy"))
        )

        var reqBody = buildReqBody(f)

        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Activities/filter").openConnection() as HttpURLConnection

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
                    if (res != null){
                        var arr = JSONArray(res)

                        data class run (
                            var date : LocalDate,
                            var distance : Double,
                        )

                        var datas = mutableListOf<run>()

                        for (i in 0 until arr.length()) {
                            var obj = arr.getJSONObject(i)

                            var d =
                                obj.getString("createdAt").split('T')[0].split('-')

                            var date = LocalDate.of(d[0].toInt(),d[1].toInt(),d[2].toInt())

                            var r = run (
                                date,
                                obj.getDouble("distance")
                            )

                            datas.add(r)
                        }

                        datas.sortBy { it.date }
                        datas.reverse()

                        binding.rv.apply {
                            layoutManager =LinearLayoutManager(this@MyRunActivity)
                            class V(var bind: ItemMyRunBinding) :RecyclerView.ViewHolder(bind.root)
                            adapter = object :RecyclerView.Adapter<V>() {
                                override fun onCreateViewHolder(
                                    parent: ViewGroup,
                                    viewType: Int
                                ): V {
                                    return V(
                                        ItemMyRunBinding.inflate(LayoutInflater.from(parent.context), parent, false)
                                    )
                                }

                                override fun getItemCount(): Int = datas.size

                                override fun onBindViewHolder(holder: V, position: Int) {
                                    var run = datas[position]

                                    holder.bind.apply {
                                        tvDate.text = run.date.format(DateTimeFormatter.ofPattern("MMM dd, yyyy"))
                                        tvDistance.text = run.distance.toString() + "Km"
                                    }
                                }

                            }
                        }

                    }
                }
            }
        }
    }
}