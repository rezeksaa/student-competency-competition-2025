package com.example.gawe06.ui.notifications

import android.graphics.BitmapFactory
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.gawe06.BaseURL
import com.example.gawe06.databinding.FragmentNotificationsBinding
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.net.HttpURLConnection
import java.net.URL

class NotificationsFragment : Fragment() {

    private var _binding: FragmentNotificationsBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentNotificationsBinding.inflate(inflater, container, false)
        val root: View = binding.root

        getData()

        return root
    }

    private fun getData() {
        GlobalScope.launch(Dispatchers.IO) {
            var url =
                URL("$BaseURL/me").openConnection() as HttpURLConnection
            var code = url.responseCode
            var res = if (code in 200..299) url.inputStream.bufferedReader().readText() else null

            withContext(Dispatchers.Main) {
                res.let {
                    if (res != null) {
                        var response = JSONObject(res)
                        var data = response.getJSONObject("data")

                        binding.tvName.text = data.getString("fullname")
                        binding.tvPhone.text = data.getString("phoneNumber")
                        binding.tvEmail.text = data.getString("email")

                        var image = data.getString("profilePicture")

                        GlobalScope.launch(Dispatchers.IO) {
                            try {

                                var bmp =
                                    BitmapFactory.decodeStream(URL("http://10.0.2.2:5000/images/$image").openStream())

                                withContext(Dispatchers.Main) {
                                    bmp.let {
                                        if (bmp != null) {
                                            binding.iv.setImageBitmap(bmp)
                                        }
                                    }
                                }
                            } catch (a : Exception) {
                                withContext(Dispatchers.Main) {

                                    Toast.makeText(
                                        requireContext(),
                                        "Photo profile not found",
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

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}