package com.example;

import java.util.ArrayList;
import java.util.List;
import org.springframework.stereotype.Repository;

@Repository
public class RecommendationRepository {
public List<Recommendation> getRecommendations(){
	List<Recommendation> recommendations = new ArrayList<Recommendation>();
	recommendations.add(new Recommendation("1", "01234", "Getting Started with kubernetes", "Jonathan Baier", "Learn Kubernetes the right way", "https://mtchouimages.blob.core.windows.net/books/Kubernetes.jpg"));
	recommendations.add(new Recommendation("2", "95201", "Learning Docker Networking", "Rajdeep Das", "Docker networking deep dive", "https://mtchouimages.blob.core.windows.net/books/DockerNetworking.jpg"));
	recommendations.add(new Recommendation("6", "95298", "Spring Microservices", "Rajesh RV", "Build scalable microservices with Spring and Docker", "https://mtchouimages.blob.core.windows.net/books/SpringMicroServices.jpg"));
	recommendations.add(new Recommendation("5", "01264", "Learning Concurrent Programming in Scala", "Aleksandar Prokopec", "Learn the art of building concurrent applications", "https://mtchouimages.blob.core.windows.net/books/Scala.jpg"));
	recommendations.add(new Recommendation("3", "23123", "Modern Authentication with AzureAD ", "Vittorio Bertocci", "Azure active directory capabilities", "https://mtchouimages.blob.core.windows.net/books/AzureAD.jpg"));
	recommendations.add(new Recommendation("4", "11201", "Microsoft Azure SQL", "Leonard G.Lobel", "Setp by step guide for developers", "https://mtchouimages.blob.core.windows.net/books/AzureSQL.jpg"));
	recommendations.add(new Recommendation("7", "28526", "Developing Azure and Web Services", "Rajdeep Das", "Exam Ref 70-487", "https://mtchouimages.blob.core.windows.net/books/AzureCert.jpg"));
	recommendations.add(new Recommendation("8", "95298", "Programming Microsoft Azure Service fabric", "Haishi Bai", "Service fabric for developers", "https://mtchouimages.blob.core.windows.net/books/ServiceFabric.jpg"));

	return recommendations;
	
}
}
