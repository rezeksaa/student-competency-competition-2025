package com.example.gawe06

import android.os.Bundle
import android.view.View
import android.widget.ShareActionProvider
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ShareCompat
import androidx.core.content.ContentProviderCompat.requireContext
import androidx.core.content.ContextCompat
import androidx.core.view.ContentInfoCompat.Flags
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import com.example.gawe06.databinding.ActivityDetailJobBinding
import com.example.gawe06.ui.home.appliedId
import com.example.gawe06.ui.home.checkData
import com.example.gawe06.ui.home.savedId
import com.google.android.material.tabs.TabLayout
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.awaitAll
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class DetailJobActivity : AppCompatActivity() {
    lateinit var binding: ActivityDetailJobBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityDetailJobBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.tvBack.setOnClickListener {
            finish()
        }

        binding.btnShare.setOnClickListener {

        }

        id = intent.getIntExtra("id", -1)

        binding.apply {
            tvResponsibilities.visibility = View.GONE
            tvResponsibilities2.visibility = View.GONE
            tvskills2.visibility = View.GONE
            tvskills.visibility = View.GONE
            tvqualifications.visibility = View.GONE
            tvqualifications2.visibility = View.GONE
        }

        if (savedId.contains(id)) {
            binding.btnSave.backgroundTintList =
                ContextCompat.getColorStateList(this@DetailJobActivity, R.color.black)
            binding.btnSave.setTextColor(
                ContextCompat.getColorStateList(
                    this@DetailJobActivity,
                    R.color.white
                )
            )
            binding.btnSave.text = "saved"
        } else {
            binding.btnSave.text = "save"
        }

        binding.btnSave.setOnClickListener {
            if (!savedId.contains(id)) {
                if (appliedId.contains(id)) {
                    Toast.makeText(
                        this@DetailJobActivity,
                        "You already apply to this job, so you can't save it",
                        Toast.LENGTH_SHORT
                    ).show()
                } else {
                    savedId.add(id)
                    binding.btnSave.backgroundTintList =
                        ContextCompat.getColorStateList(this@DetailJobActivity, R.color.black)
                    binding.btnSave.setTextColor(
                        ContextCompat.getColorStateList(
                            this@DetailJobActivity,
                            R.color.white
                        )
                    )
                    binding.btnSave.text = "saved"
                }
            } else {
                savedId.remove(id)
                binding.btnSave.text = "save"
            }
        }

        binding.btnApply.setOnClickListener {
            println(appliedId)
            println(id)

            if (!com.example.gawe06.ui.home.appliedId.contains(id)) {
                GlobalScope.launch(Dispatchers.IO) {
                    var url =
                        URL("$BaseURL/jobs/$id/apply").openConnection() as HttpURLConnection

                    url.requestMethod = "POST"

                    var code = url.responseCode
                    var res =
                        if (code in 200..299) url.inputStream.bufferedReader().readText() else null

                    withContext(Dispatchers.Main) {
                        res.let {
                            if (res != null) {
                                Toast.makeText(
                                    this@DetailJobActivity,
                                    "You applied to this job successfully",
                                    Toast.LENGTH_SHORT
                                ).show()
                            }
                        }
                    }
                    checkData()

                }
            } else {
                Toast.makeText(
                    this@DetailJobActivity,
                    "You already apply to this job!",
                    Toast.LENGTH_SHORT
                ).show()
            }
        }

        binding.tab.addOnTabSelectedListener(object : TabLayout.OnTabSelectedListener {
            override fun onTabSelected(tab: TabLayout.Tab?) {
                if (binding.tab.selectedTabPosition == 0) {
                    binding.apply {
                        tvResponsibilities.visibility = View.GONE
                        tvResponsibilities2.visibility = View.GONE
                        tvskills2.visibility = View.GONE
                        tvskills.visibility = View.GONE
                        tvqualifications.visibility = View.GONE
                        tvqualifications2.visibility = View.GONE

                        tvOverview.visibility = View.VISIBLE
                        tvOverview2.visibility = View.VISIBLE
                    }
                } else {
                    binding.apply {
                        tvResponsibilities.visibility = View.VISIBLE
                        tvResponsibilities2.visibility = View.VISIBLE
                        tvskills2.visibility = View.VISIBLE
                        tvskills.visibility = View.VISIBLE
                        tvqualifications.visibility = View.VISIBLE
                        tvqualifications2.visibility = View.VISIBLE

                        tvOverview.visibility = View.GONE
                        tvOverview2.visibility = View.GONE
                    }
                }
            }

            override fun onTabUnselected(tab: TabLayout.Tab?) {
            }

            override fun onTabReselected(tab: TabLayout.Tab?) {
            }

        })

        getData()
    }

    var id = 0

    private fun getData() {
        GlobalScope.launch(Dispatchers.IO) {
            var url =
                URL("$BaseURL/jobs/$id").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200..299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var response = JSONObject(res)
                        var data = response.getJSONObject("data")

                        binding.tvJobName.text = data.getString("name")
                        binding.tvCompany.text = data.getJSONObject("company").getString("name")
                        binding.tvLocation.text =
                            data.getString("locationType") + "(${data.getString("locationRegion")})"
                        binding.tvExpirience.text =
                            "Min. ${data.getString("yearOfExperience")} years of experience"

                        binding.apply {
                            tvOverview.text = data.getJSONObject("company").getString("overview")
                            tvResponsibilities.text = data.getString("responsibilities")
                            tvqualifications.text = data.getString("qualifications")
                            tvskills.text = data.getString("softSkills")
                        }
                    }
                }
            }
        }
    }
}