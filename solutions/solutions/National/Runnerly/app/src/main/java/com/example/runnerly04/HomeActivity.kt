package com.example.runnerly04

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.runnerly04.databinding.ActivityHomeBinding
import com.example.runnerly04.databinding.ItemRecentBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.format.DateTimeFormatter
import kotlin.math.roundToInt

class HomeActivity : AppCompatActivity() {
    lateinit var binding : ActivityHomeBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityHomeBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.logout.setOnClickListener {
            finish()
        }

        getData()

        binding.logRun.setOnClickListener {
            startActivity(
                Intent(this@HomeActivity, LogRunActivity::class.java)
            )
        }

        binding.thisWeeek.setOnClickListener {
            startActivity(
                Intent(this@HomeActivity, MyRunActivity::class.java)
            )
        }

        binding.chalengges.setOnClickListener {
            startActivity(
                Intent(this@HomeActivity, ChalenggesActivity::class.java)
            )
        }
    }
    override fun onResume() {
        super.onResume()
        getData()
    }

    private fun getData() {
        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Activities/stats?userId=$id").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200 .. 299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var obj = JSONObject(res)

                        binding.tvTime.text = obj.getString("totalDuration")
                        binding.tvPace.text = Math.round(obj.getDouble("averagePace")).toString() + "/Km"
                        binding.tvKm.text = obj.getDouble("totalDistance").toString() + "Km"

                        var arr = obj.getJSONArray("recentActivities")

                        data class run (
                            var date : LocalDate,
                            var distance : Double,
                            var pace : Double,
                            var time : String
                        )

                        var datas = mutableListOf<run>()

                        for (i in 0 until arr.length()) {
                            var obj = arr.getJSONObject(i)

                            var d =
                                obj.getString("createdAt").split('T')[0].split('-')

                            var date = LocalDate.of(d[0].toInt(),d[1].toInt(),d[2].toInt())

                            var r = run (
                                date,
                                obj.getDouble("distance"),
                                obj.getDouble("averagePace"),
                                obj.getString("duration")
                            )

                            datas.add(r)
                        }

                        datas.sortBy { it.date }
                        datas.reverse()

                        binding.rv.apply {
                            layoutManager = LinearLayoutManager(this@HomeActivity)
                            class V (var bind : ItemRecentBinding) : RecyclerView.ViewHolder(bind.root)
                            adapter = object :RecyclerView.Adapter<V>() {
                                override fun onCreateViewHolder(
                                    parent: ViewGroup,
                                    viewType: Int
                                ): V {
                                    return V(
                                        ItemRecentBinding.inflate(LayoutInflater.from(parent.context), parent, false)
                                    )
                                }

                                override fun getItemCount(): Int = datas.size

                                override fun onBindViewHolder(holder: V, position: Int) {
                                    var data = datas[position]

                                    holder.bind.apply {
                                        var date = data.date

                                        if (date == LocalDate.now()) {
                                            tvDate.text = "Today"
                                        } else {
                                            tvDate.text = date.format(DateTimeFormatter.ofPattern("MMM dd"))
                                        }

                                        tvDistance.text = data.distance.toString() + "Km"
                                        tvPace.text = Math.round(data.pace).toString() + "/Km"
                                        tvTime.text = data.time
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