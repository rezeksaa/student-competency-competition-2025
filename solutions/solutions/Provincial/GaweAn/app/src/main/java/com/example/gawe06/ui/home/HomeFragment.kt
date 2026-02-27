package com.example.gawe06.ui.home

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.core.widget.addTextChangedListener
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.gawe06.BaseURL
import com.example.gawe06.DetailJobActivity
import com.example.gawe06.R
import com.example.gawe06.databinding.FragmentHomeBinding
import com.example.gawe06.databinding.ItemExploreBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONArray
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

var savedId = mutableListOf<Int>()
var appliedId = mutableListOf<Int>()

class HomeFragment : Fragment() {

    private var _binding: FragmentHomeBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        val root: View = binding.root

        getData("")

        binding.etSearch.addTextChangedListener {
            if (binding.btnAll.backgroundTintList == ContextCompat.getColorStateList(
                    requireContext(),
                    R.color.black
                )
            ) {
                getData("")
            } else if (binding.btnOnsite.backgroundTintList == ContextCompat.getColorStateList(
                    requireContext(),
                    R.color.black
                )
            ) {
                getData("onsite")
            } else {
                getData("remote")
            }
        }

        binding.btnOnsite.setOnClickListener {
            getData("onsite")
            binding.apply {
                btnAll.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.white)
                btnAll.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.black
                    )
                )
                btnOnsite.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.black)
                btnOnsite.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.white
                    )
                )
                btnRemote.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.white)
                btnRemote.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.black
                    )
                )
            }
        }

        binding.btnRemote.setOnClickListener {
            getData("remote")
            binding.apply {
                btnAll.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.white)
                btnAll.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.black
                    )
                )
                btnRemote.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.black)
                btnRemote.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.white
                    )
                )
                btnOnsite.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.white)
                btnOnsite.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.black
                    )
                )
            }
        }

        binding.btnAll.setOnClickListener {

            getData("")
            binding.apply {
                btnOnsite.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.white)
                btnOnsite.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.black
                    )
                )
                btnAll.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.black)
                btnAll.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.white
                    )
                )
                btnRemote.backgroundTintList =
                    ContextCompat.getColorStateList(requireContext(), R.color.white)
                btnRemote.setTextColor(
                    ContextCompat.getColorStateList(
                        requireContext(),
                        R.color.black
                    )
                )
            }
        }

        checkData()

        return root
    }

    override fun onResume() {
        super.onResume()
        refr()
    }

    fun refr() {
        getData("")
        binding.apply {
            btnOnsite.backgroundTintList =
                ContextCompat.getColorStateList(requireContext(), R.color.white)
            btnOnsite.setTextColor(
                ContextCompat.getColorStateList(
                    requireContext(),
                    R.color.black
                )
            )
            btnAll.backgroundTintList =
                ContextCompat.getColorStateList(requireContext(), R.color.black)
            btnAll.setTextColor(
                ContextCompat.getColorStateList(
                    requireContext(),
                    R.color.white
                )
            )
            btnRemote.backgroundTintList =
                ContextCompat.getColorStateList(requireContext(), R.color.white)
            btnRemote.setTextColor(
                ContextCompat.getColorStateList(
                    requireContext(),
                    R.color.black
                )
            )
        }
    }

    private fun getData(location: String) {
        GlobalScope.launch(Dispatchers.IO) {
            var url =
                URL("$BaseURL/jobs?search=${binding.etSearch.text}&location=$location").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200..299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var response = JSONObject(res)
                        var datas = response.getJSONArray("data")

                        binding.rv.apply {
                            layoutManager = LinearLayoutManager(this.context)
                            class V(var bind : ItemExploreBinding) : RecyclerView.ViewHolder(bind.root)
                            adapter = object : RecyclerView.Adapter<V>() {
                                override fun onCreateViewHolder(
                                    parent: ViewGroup,
                                    viewType: Int
                                ): V {
                                    return V(
                                    ItemExploreBinding.inflate(LayoutInflater.from(parent.context), parent, false))
                                }

                                override fun getItemCount(): Int = datas.length()

                                override fun onBindViewHolder(holder: V, position: Int) {
                                    var data = datas.getJSONObject(position)

                                    holder.bind.apply {
                                        tvJobName.text = data.getString("name")
                                        tvCompany.text = data.getJSONObject("company").getString("name")
                                        tvLocation.text = data.getString("locationType") + "(${data.getString("locationRegion")})"
                                        tvExpirience.text = "Min. ${data.getString("yearOfExperience")} years of experience"

                                        root.setOnClickListener {
                                            startActivity(
                                                Intent(requireParentFragment().context, DetailJobActivity::class.java).putExtra("id", data.getInt("id"))
                                            )
                                        }

                                        if (savedId.contains(data.getInt("id"))) {
                                            btnSave.backgroundTintList = ContextCompat.getColorStateList(requireContext(), R.color.black)
                                            btnSave.setTextColor(ContextCompat.getColorStateList(requireContext(), R.color.white))
                                            btnSave.text = "saved"
                                        }

                                        btnSave.setOnClickListener {
                                            if (!savedId.contains(data.getInt("id"))) {
                                                if (appliedId.contains(data.getInt("id"))) {
                                                    Toast.makeText(requireContext(), "You already apply to this job, so you can't save it", Toast.LENGTH_SHORT).show()
                                                } else {
                                                    savedId.add(data.getInt("id"))
                                                    btnSave.backgroundTintList = ContextCompat.getColorStateList(requireContext(), R.color.black)
                                                    btnSave.setTextColor(ContextCompat.getColorStateList(requireContext(), R.color.white))
                                                    btnSave.text = "saved"
                                                }
                                            } else {
                                                savedId.remove(data.getInt("id"))
                                                getData(location)
                                            }
                                        }

                                        btnApply.setOnClickListener {
                                            if (!com.example.gawe06.ui.home.appliedId.contains(data.getInt("id"))) {
                                                GlobalScope.launch(Dispatchers.IO) {
                                                    var url =
                                                        URL("$BaseURL/jobs/${data.getInt("id")}/apply").openConnection() as HttpURLConnection

                                                    url.requestMethod = "POST"

                                                    var code = url.responseCode
                                                    var res = if (code in 200..299) url.inputStream.bufferedReader().readText() else null

                                                    withContext(Dispatchers.Main) {
                                                        res.let {
                                                            if (res != null) {
                                                                Toast.makeText(requireContext(), "You applied to this job successfully", Toast.LENGTH_SHORT).show()
                                                            }
                                                        }
                                                    }

                                                    checkData()
                                                    refr()
                                                }
                                            } else {
                                                Toast.makeText(requireContext(), "You already apply to this job!", Toast.LENGTH_SHORT).show()
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



    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}


 fun checkData() {
    appliedId.clear()

    GlobalScope.launch(Dispatchers.IO) {
        var url =
            URL("$BaseURL/job-applications").openConnection() as HttpURLConnection
        var code = url.responseCode
        var res = if (code in 200..299) url.inputStream.bufferedReader().readText() else null

        println(listOf(url, code, res))

        withContext(Dispatchers.Main) {
            res.let {
                if (res != null) {
                    var response = JSONObject(res)
                    var data = response.getJSONArray("data")

                    for (i in 0 until data.length()) {
                        var jsonObject = data.getJSONObject(i)
                        appliedId.add(jsonObject.getJSONObject("job").getInt("id"))
                    }
                }
            }
        }
    }
}