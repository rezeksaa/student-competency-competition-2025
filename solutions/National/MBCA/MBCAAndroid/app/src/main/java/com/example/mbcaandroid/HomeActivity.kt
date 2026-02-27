package com.example.mbcaandroid

import android.content.Intent
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.graphics.Canvas
import android.os.Bundle
import android.os.Environment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.mbcaandroid.databinding.ActivityHomeBinding
import com.example.mbcaandroid.databinding.ItemEventBinding
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONArray
import java.io.File
import java.io.FileOutputStream
import java.net.HttpURLConnection
import java.net.URL

class HomeActivity : AppCompatActivity() {
    lateinit var binding : ActivityHomeBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityHomeBinding.inflate(layoutInflater)
        setContentView(binding.root)

        getData()

        binding.tab.addOnTabSelectedListener(object :TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                if (tab?.position == 0) {
                    binding.eventLayout.visibility = View.VISIBLE
                    binding.ticketLayout.visibility = View.GONE
                } else {
                    binding.eventLayout.visibility = View.GONE
                    binding.ticketLayout.visibility = View.VISIBLE
                    getTicketData()
                }
            }

            override fun onTabUnselected(tab: TabLayout.Tab?) {

            }

            override fun onTabReselected(tab: TabLayout.Tab?) {

            }

        })

    }

    private fun getTicketData() {
        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Ticket").openConnection() as HttpURLConnection
            url.setRequestProperty("Authorization", "Bearer $token")
            var code = url.responseCode
            var res = if (code in 200 .. 299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var arr = JSONArray(res)

                        binding.rv2.apply {
                            layoutManager = LinearLayoutManager(this@HomeActivity)
                            class V (var bind : ItemEventBinding) :RecyclerView.ViewHolder(bind.root)
                            adapter = object :RecyclerView.Adapter<V>() {
                                override fun onCreateViewHolder(
                                    parent: ViewGroup,
                                    viewType: Int
                                ): V {
                                    return V(
                                        ItemEventBinding.inflate(LayoutInflater.from(parent.context), parent, false)
                                    )
                                }

                                override fun getItemCount(): Int = arr.length()

                                override fun onBindViewHolder(holder: V, position: Int) {
                                    var obj = arr.getJSONObject(position)

                                    holder.bind.apply {
                                        tvName.text = obj.getString("title")
                                        tvPrice.text = "T" + obj.getString("id").toString()
                                        tvDate.text = obj.getString("date")

                                        btnDownload.setOnClickListener {
                                            btnDownload.visibility = View.GONE

                                            var bmp = Bitmap.createBitmap(holder.bind.root.width, holder.bind.root.height, Bitmap.Config.ARGB_8888)
                                            var canvas = Canvas(bmp)
                                            holder.bind.root.draw(canvas)

                                            var file = File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS), "ticket " + obj.getString("title") + "T" + obj.getString("id").toString() + ".jpg")
                                            FileOutputStream(file).use {
                                                bmp.compress(Bitmap.CompressFormat.JPEG, 100, it)
                                            }

                                            Toast.makeText(this@HomeActivity, "The ticket has been successfully downloaded to your donwload folder!", Toast.LENGTH_SHORT).show()

                                            btnDownload.visibility = View.VISIBLE
                                        }

                                        GlobalScope.launch(Dispatchers.IO) {
                                            var stream = URL("http://10.0.2.2:5000/Image/" + obj.getString("banner")).openConnection() as HttpURLConnection

                                            if (stream.responseCode == 200) {

                                                var bmp = BitmapFactory.decodeStream(stream.inputStream)

                                                withContext(Dispatchers.Main) {

                                                    bmp.let {
                                                        if (bmp != null) {
                                                            iv.setImageBitmap(it)
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
            }
        }
    }

    private fun getData() {
        GlobalScope.launch(Dispatchers.IO) {
            var url = URL("$BaseURL/Events").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200 .. 299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var arr = JSONArray(res)

                        binding.rv.apply {
                            layoutManager = LinearLayoutManager(this@HomeActivity)
                            class V (var bind : ItemEventBinding) :RecyclerView.ViewHolder(bind.root)
                            adapter = object :RecyclerView.Adapter<V>() {
                                override fun onCreateViewHolder(
                                    parent: ViewGroup,
                                    viewType: Int
                                ): V {
                                    return V(
                                        ItemEventBinding.inflate(LayoutInflater.from(parent.context), parent, false)
                                    )
                                }

                                override fun getItemCount(): Int = arr.length()

                                override fun onBindViewHolder(holder: V, position: Int) {
                                    var obj = arr.getJSONObject(position)

                                    holder.bind.apply {
                                        tvName.text = obj.getString("title")
                                        tvPrice.text = "$" + obj.getDouble("price").toString() + "/person"
                                        tvDate.text = obj.getString("date")
                                        btnDownload.visibility = View.GONE

                                        holder.bind.root.setOnClickListener {
                                            startActivity(
                                                Intent(this@HomeActivity, EventDetailActivity::class.java).putExtra("obj", obj.toString())
                                            )
                                        }

                                        GlobalScope.launch(Dispatchers.IO) {
                                            var stream = URL("http://10.0.2.2:5000/Image/" + obj.getString("banner")).openConnection() as HttpURLConnection

                                            if (stream.responseCode == 200) {

                                                var bmp = BitmapFactory.decodeStream(stream.inputStream)

                                                withContext(Dispatchers.Main) {

                                                    bmp.let {
                                                        if (bmp != null) {
                                                            iv.setImageBitmap(it)
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
            }
        }
    }
}