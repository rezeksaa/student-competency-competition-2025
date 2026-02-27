package com.example.runnerly04

import android.content.Intent
import android.os.Bundle
import android.text.Layout
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import androidx.recyclerview.widget.RecyclerView.Adapter
import com.example.runnerly04.databinding.ActivityChalenggesBinding
import com.example.runnerly04.databinding.ItemChalenggeBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class ChalenggesActivity : AppCompatActivity() {
    lateinit var binding : ActivityChalenggesBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityChalenggesBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.toolbar.setNavigationOnClickListener { finish() }

        getData()
    }

    private fun getData() {
        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Challenges/active").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200 .. 299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var obj = JSONObject(res)
                        var arr = obj.getJSONArray("challenges")

                        binding.rv.apply {
                            layoutManager = LinearLayoutManager(this@ChalenggesActivity)
                            class V(var bind: ItemChalenggeBinding) : RecyclerView.ViewHolder(bind.root)
                            adapter = object :Adapter<V>() {
                                override fun onCreateViewHolder(
                                    parent: ViewGroup,
                                    viewType: Int
                                ): V {
                                    return V(
                                        ItemChalenggeBinding.inflate(LayoutInflater.from(parent.context), parent, false)
                                    )
                                }

                                override fun getItemCount(): Int = arr.length()

                                override fun onBindViewHolder(holder: V, position: Int) {
                                    var obj = arr.getJSONObject(position)

                                    var d = obj.getString("endDate").split('T')[0].split('-')
                                    var date = LocalDate.of(d[0].toInt(), d[1].toInt(), d[2].toInt())

                                    holder.bind.apply {
                                        tvName.text = obj.getString("title")
                                        tvDesc.text = obj.getString("description")
                                        tvTargetDistance.text = "Target Distance\n" + obj.getDouble("targetValue").toString()
                                        tvDeeadline.text = "Deadline\n" + date.format(
                                            DateTimeFormatter.ofPattern("MMM dd"))

                                        btnLeaderboard.setOnClickListener {
                                            startActivity(
                                                Intent(this@ChalenggesActivity, LeaderboardActivity::class.java).putExtra("id", obj.getInt("id"))
                                            )
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
}