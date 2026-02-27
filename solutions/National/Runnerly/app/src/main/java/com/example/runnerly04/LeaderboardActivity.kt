package com.example.runnerly04

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.runnerly04.databinding.ActivityLadeerboardBinding
import com.example.runnerly04.databinding.ItemLeaderboardBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL
import java.security.DigestOutputStream

class LeaderboardActivity : AppCompatActivity() {
    lateinit var binding: ActivityLadeerboardBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityLadeerboardBinding.inflate(layoutInflater)
        setContentView(binding.root)

        getData()

        binding.toolbar.setNavigationOnClickListener {
            finish()
        }
    }

    private fun getData() {
        var idChalengge = intent.getIntExtra("id", -1)

        GlobalScope.launch(Dispatchers.IO) {
            var url =
                URL("$BaseURL/Challenges/$idChalengge/leaderboard").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200..299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var obj = JSONObject(res)
                        var arr = obj.getJSONArray("participants")

                        binding.rv.apply {
                            layoutManager = LinearLayoutManager(this@LeaderboardActivity)
                            class V(var bind: ItemLeaderboardBinding) :
                                RecyclerView.ViewHolder(bind.root)
                            adapter = object : RecyclerView.Adapter<V>() {
                                override fun onCreateViewHolder(
                                    parent: ViewGroup,
                                    viewType: Int
                                ): V {
                                    return V(
                                        ItemLeaderboardBinding.inflate(
                                            LayoutInflater.from(parent.context),
                                            parent,
                                            false
                                        )
                                    )
                                }

                                override fun getItemCount(): Int = arr.length()

                                override fun onBindViewHolder(holder: V, position: Int) {
                                    var obj = arr.getJSONObject(position)

                                    holder.bind.apply {
                                        tvName.text = obj.getString("username")
                                        tvNum.text = (position + 1).toString()
                                        tvPercentage.text =
                                            obj.getDouble("progressPercentage").toString() + "%"
                                        bar.setProgress(
                                            Math.round(obj.getDouble("progressPercentage")).toInt(),
                                            true
                                        )

                                        if (obj.getDouble("progressPercentage").toInt() != 100) {
                                            trophy.visibility = View.GONE
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