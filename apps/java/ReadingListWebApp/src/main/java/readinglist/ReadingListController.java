package readinglist;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.servlet.ModelAndView;

@Controller
@ConfigurationProperties(prefix="endpoint")
public class ReadingListController {
	private final Logger logger = LoggerFactory.getLogger(this.getClass());
	private static final String reader = "Ravi Tella";
	@Value("${externalAPI.recommendationAPIURL}")
	private String recommendationAPIURL ;

	private ReadingListRepository readingListRepository;
	private String ipaddress;

	@Autowired
	public ReadingListController(ReadingListRepository readingListRepository) {
		this.readingListRepository = readingListRepository;
		this.ipaddress = getServerIP();
	}

	@RequestMapping(value = "/readingList", method = RequestMethod.GET)
	public String readersBooks(Model model) {
		List<Book> readingList = readingListRepository.findByReader(reader);
		logger.info("The server IP is : " + this.ipaddress);

		model.addAttribute("serverIP", this.ipaddress);
		if (readingList != null) {
			model.addAttribute("books", readingList);
		}
		RestTemplate restTemplate = new RestTemplate();
		        List<Recommendation> recommendations = restTemplate.getForObject( recommendationAPIURL, List.class);
		        logger.info("Number of recommendations: "+Integer.toString(recommendations.size()));
        model.addAttribute("recommendations",recommendations);
		return "readingList";
	}

	@RequestMapping(value = "/add", method = RequestMethod.POST)
	public String addToReadingList(Book book) {
		book.setReader(reader);
		readingListRepository.save(book);
		logger.info("Added a Book to the reading list: " + book);
		return "redirect:/readingList";
	}

	@RequestMapping(value = "/delete/{id}", method = RequestMethod.GET)
	public String deleteFromReadingList(@PathVariable String id) {
		readingListRepository.delete(Long.parseLong(id));
		logger.info("Deleted a Book from the reading list: " + "Book ID:"+id);
		return "redirect:/readingList";
	}

	@RequestMapping(value = "/edit/{id}", method = RequestMethod.GET)
	public ModelAndView editReadingListView(@PathVariable String id) {
		ModelAndView model = new ModelAndView("editReadingList");
		Book book = readingListRepository.findOne(Long.parseLong(id));
		model.addObject("book", book);
		model.addObject("serverIP", this.ipaddress);
		return model;
	}

	@RequestMapping(value = "/edit", method = RequestMethod.POST)
	public String editReadingListItem(Book updatedBook, @RequestParam String action) {

		if (action.equals("update")) {
			Book book = readingListRepository.findOne(updatedBook.getId());
			book.setTitle(updatedBook.getTitle());
			book.setAuthor(updatedBook.getAuthor());
			book.setIsbn(updatedBook.getIsbn());
			book.setDescription(updatedBook.getDescription());
			readingListRepository.save(book);
			logger.info("Edited a Book from the reading list: " + book);

		}

		return "redirect:/readingList";
	}

	private String getServerIP() {
		InetAddress ip = null;
		try {
			ip = InetAddress.getLocalHost();
		} catch (UnknownHostException e) {
			e.printStackTrace();
		}
		String[] values = ip.toString().split("/");
		return values[1];
	}
}
