package com.example.gawe06.ui.dashboard

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.gawe06.BaseURL
import com.example.gawe06.DetailJobActivity
import com.example.gawe06.R
import com.example.gawe06.databinding.FragmentDashboardBinding
import com.example.gawe06.databinding.ItemExploreBinding
import com.example.gawe06.ui.home.checkData
import com.example.gawe06.ui.home.savedId
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class DashboardFragment : Fragment() {

    private var _binding: FragmentDashboardBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentDashboardBinding.inflate(inflater, container, false)
        val root: View = binding.root

        getData()

        binding.tab.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                getData()

            }

            override fun onTabUnselected(tab: TabLayout.Tab?) {
            }

            override fun onTabReselected(tab: TabLayout.Tab?) {
            }

        })

        return root
    }

    private fun getData() {
        if (binding.tab.selectedTabPosition == 0) {
            binding.rv.apply {
                layoutManager = LinearLayoutManager(this.context)
                class V(var bind: ItemExploreBinding) : RecyclerView.ViewHolder(bind.root)
                adapter = object : RecyclerView.Adapter<V>() {
                    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): V {
                        return V(
                            ItemExploreBinding.inflate(
                                LayoutInflater.from(parent.context),
                                parent,
                                false
                            )
                        )
                    }

                    override fun getItemCount(): Int = savedId.count()

                    override fun onBindViewHolder(holder: V, position: Int) {
                        var curId = savedId[position]

                        GlobalScope.launch(Dispatchers.IO) {
                            var url =
                                URL("$BaseURL/jobs/$curId").openConnection() as HttpURLConnection
                            var code = url.responseCode
                            var res = if (code in 200..299) url.inputStream.bufferedReader()
                                .readText() else null

                            withContext(Dispatchers.Main) {
                                res.let {
                                    if (res != null) {
                                        var response = JSONObject(res)
                                        var data = response.getJSONObject("data")
                                        holder.bind.apply {
                                            tvJobName.text = data.getString("name")
                                            tvCompany.text =
                                                data.getJSONObject("company").getString("name")
                                            tvLocation.text = data.getString("locationType") + "(${
                                                data.getString("locationRegion")
                                            })"
                                            tvExpirience.text =
                                                "Min. ${data.getString("yearOfExperience")} years of experience"

                                            root.setOnClickListener {
                                                startActivity(
                                                    Intent(
                                                        requireParentFragment().context,
                                                        DetailJobActivity::class.java
                                                    ).putExtra("id", data.getInt("id"))
                                                )
                                            }

                                            if (savedId.contains(data.getInt("id"))) {
                                                btnSave.backgroundTintList =
                                                    ContextCompat.getColorStateList(
                                                        requireContext(),
                                                        R.color.black
                                                    )
                                                btnSave.setTextColor(
                                                    ContextCompat.getColorStateList(
                                                        requireContext(),
                                                        R.color.white
                                                    )
                                                )
                                                btnSave.text = "saved"
                                            }

                                            btnSave.setOnClickListener {
                                                if (!savedId.contains(data.getInt("id"))) {
                                                    savedId.add(data.getInt("id"))
                                                    btnSave.backgroundTintList =
                                                        ContextCompat.getColorStateList(
                                                            requireContext(),
                                                            R.color.black
                                                        )
                                                    btnSave.setTextColor(
                                                        ContextCompat.getColorStateList(
                                                            requireContext(),
                                                            R.color.white
                                                        )
                                                    )
                                                    btnSave.text = "saved"
                                                } else {
                                                    savedId.remove(data.getInt("id"))
                                                    getData()
                                                }
                                            }

                                            btnApply.setOnClickListener {
                                                if (!com.example.gawe06.ui.home.appliedId.contains(
                                                        data.getInt("id")
                                                    )
                                                ) {
                                                    GlobalScope.launch(Dispatchers.IO) {
                                                        var url =
                                                            URL("$BaseURL/jobs/${data.getInt("id")}/apply").openConnection() as HttpURLConnection

                                                        url.requestMethod = "POST"

                                                        var code = url.responseCode
                                                        var res =
                                                            if (code in 200..299) url.inputStream.bufferedReader()
                                                                .readText() else null

                                                        withContext(Dispatchers.Main) {
                                                            res.let {
                                                                if (res != null) {
                                                                    Toast.makeText(
                                                                        requireContext(),
                                                                        "You applied to this job successfully",
                                                                        Toast.LENGTH_SHORT
                                                                    ).show()
                                                                    checkData()
                                                                    savedId.remove(data.getInt("id"))
                                                                    getData()
                                                                }
                                                            }
                                                        }


                                                    }
                                                } else {
                                                    Toast.makeText(
                                                        requireContext(),
                                                        "You already apply to this job!",
                                                        Toast.LENGTH_SHORT
                                                    ).show()
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
        } else {
            GlobalScope.launch(Dispatchers.IO) {
                var url =
                    URL("$BaseURL/job-applications").openConnection() as HttpURLConnection
                var code = url.responseCode
                var res = if (code in 200..299) url.inputStream.bufferedReader()
                    .readText() else null

                withContext(Dispatchers.Main) {

                    var response = JSONObject(res)
                    var data = response.getJSONArray("data")

                    res.let {
                        if (res != null) {
                            binding.rv.apply {
                                layoutManager = LinearLayoutManager(this.context)
                                class V(var bind: ItemExploreBinding) : RecyclerView.ViewHolder(bind.root)
                                adapter = object : RecyclerView.Adapter<V>() {
                                    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): V {
                                        return V(
                                            ItemExploreBinding.inflate(
                                                LayoutInflater.from(parent.context),
                                                parent,
                                                false
                                            )
                                        )
                                    }

                                    override fun getItemCount(): Int = data.length()

                                    override fun onBindViewHolder(holder: V, position: Int) {
                                        var job = data.getJSONObject(position).getJSONObject("job")

                                        var curId = job.getInt("id")

                                        GlobalScope.launch(Dispatchers.IO) {
                                            var url =
                                                URL("$BaseURL/jobs/$curId").openConnection() as HttpURLConnection
                                            var code = url.responseCode
                                            var res = if (code in 200..299) url.inputStream.bufferedReader()
                                                .readText() else null

                                            withContext(Dispatchers.Main) {
                                                res.let {
                                                    if (res != null) {
                                                        var response = JSONObject(res)
                                                        var data = response.getJSONObject("data")
                                                        holder.bind.apply {
                                                            tvJobName.text = data.getString("name")
                                                            tvCompany.text =
                                                                data.getJSONObject("company").getString("name")
                                                            tvLocation.text = data.getString("locationType") + "(${
                                                                data.getString("locationRegion")
                                                            })"
                                                            tvExpirience.text =
                                                                "Min. ${data.getString("yearOfExperience")} years of experience"

                                                            root.setOnClickListener {
                                                                startActivity(
                                                                    Intent(
                                                                        requireParentFragment().context,
                                                                        DetailJobActivity::class.java
                                                                    ).putExtra("id", data.getInt("id"))
                                                                )
                                                            }

                                                            btnSave.visibility = View.GONE

                                                            btnApply.text = "Waiting For Confirmation"
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

    override fun onResume() {
        super.onResume()
        getData()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}