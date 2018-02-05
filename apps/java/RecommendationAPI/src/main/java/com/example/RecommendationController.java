package com.example;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class RecommendationController {
	private final Logger logger = LoggerFactory.getLogger(this.getClass());
	private RecommendationRepository recommendationRepository;

	@Autowired
	public RecommendationController(com.example.RecommendationRepository recommendationRepository) {
		super();
		this.recommendationRepository = recommendationRepository;
	}

	@RequestMapping(value = "/recommendations", method = RequestMethod.GET)
	public List<Recommendation> readersBooks(Model model) {
		List<Recommendation> recommendationList = recommendationRepository.getRecommendations();

		model.addAttribute("recommendationList", recommendationList);

		return recommendationList;
	}
}
