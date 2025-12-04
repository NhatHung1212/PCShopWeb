// Live Search Functionality
document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.querySelector('.search-input');
    const searchForm = document.querySelector('.search-form');
    let searchTimeout;
    const SEARCH_DELAY = 500; // milliseconds
    
    if (!searchInput || !searchForm) return;
    
    // Create live search results container
    const searchResults = document.createElement('div');
    searchResults.className = 'live-search-results';
    searchResults.style.cssText = `
        position: absolute;
        top: calc(100% + 10px);
        left: 0;
        right: 0;
        background: linear-gradient(180deg, #1a1a1a 0%, #0f0f0f 100%);
        border: 1px solid #2a2a2a;
        border-radius: 12px;
        max-height: 400px;
        overflow-y: auto;
        display: none;
        z-index: 1001;
        box-shadow: 0 10px 40px rgba(0, 0, 0, 0.8);
    `;
    
    const searchContainer = document.querySelector('.search-container');
    searchContainer.style.position = 'relative';
    searchContainer.appendChild(searchResults);
    
    // Live search on input
    searchInput.addEventListener('input', function() {
        const searchTerm = this.value.trim();
        
        // Clear previous timeout
        clearTimeout(searchTimeout);
        
        if (searchTerm.length < 2) {
            searchResults.style.display = 'none';
            return;
        }
        
        // Set new timeout for search
        searchTimeout = setTimeout(() => {
            performLiveSearch(searchTerm);
        }, SEARCH_DELAY);
    });
    
    // Perform live search
    async function performLiveSearch(searchTerm) {
        searchResults.innerHTML = '<div style="padding: 20px; text-align: center; color: #888;"><i class="fas fa-spinner fa-spin"></i> Đang tìm kiếm...</div>';
        searchResults.style.display = 'block';
        
        try {
            const response = await fetch(`/Product/Index?searchTerm=${encodeURIComponent(searchTerm)}`);
            const html = await response.text();
            
            // Parse HTML to extract products
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');
            const products = doc.querySelectorAll('.product-card');
            
            if (products.length === 0) {
                searchResults.innerHTML = `
                    <div style="padding: 30px; text-align: center; color: #888;">
                        <i class="fas fa-search" style="font-size: 48px; margin-bottom: 15px; opacity: 0.3;"></i>
                        <p>Không tìm thấy sản phẩm nào phù hợp với "<strong style="color: #fff;">${searchTerm}</strong>"</p>
                    </div>
                `;
            } else {
                displaySearchResults(products, searchTerm);
            }
        } catch (error) {
            console.error('Search error:', error);
            searchResults.innerHTML = '<div style="padding: 20px; text-align: center; color: #ef4444;">Có lỗi xảy ra khi tìm kiếm</div>';
        }
    }
    
    // Display search results
    function displaySearchResults(products, searchTerm) {
        const maxResults = 5;
        const limitedProducts = Array.from(products).slice(0, maxResults);
        
        let resultsHTML = '<div style="padding: 15px 20px; border-bottom: 1px solid #2a2a2a; color: #888; font-size: 13px;">';
        resultsHTML += `Tìm thấy ${products.length} sản phẩm${products.length > maxResults ? ` (hiển thị ${maxResults})` : ''}`;
        resultsHTML += '</div>';
        
        limitedProducts.forEach(product => {
            const link = product.querySelector('a');
            const img = product.querySelector('.product-image');
            const name = product.querySelector('.product-name');
            const price = product.querySelector('.product-price');
            
            if (link && name && price) {
                const href = link.getAttribute('href');
                const imgSrc = img ? img.getAttribute('src') : '';
                
                resultsHTML += `
                    <a href="${href}" class="live-search-item" style="
                        display: flex;
                        align-items: center;
                        gap: 15px;
                        padding: 15px 20px;
                        text-decoration: none;
                        color: #fff;
                        border-bottom: 1px solid #1a1a1a;
                        transition: all 0.3s;
                    ">
                        <img src="${imgSrc}" alt="${name.textContent}" style="
                            width: 60px;
                            height: 60px;
                            object-fit: cover;
                            border-radius: 8px;
                            background: #0a0a0a;
                        ">
                        <div style="flex: 1; min-width: 0;">
                            <div style="font-weight: 600; margin-bottom: 5px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">
                                ${name.textContent}
                            </div>
                            <div style="color: #a855f7; font-weight: 700; font-size: 15px;">
                                ${price.textContent}
                            </div>
                        </div>
                        <i class="fas fa-arrow-right" style="color: #555;"></i>
                    </a>
                `;
            }
        });
        
        if (products.length > maxResults) {
            resultsHTML += `
                <a href="/Product/Index?searchTerm=${encodeURIComponent(searchTerm)}" style="
                    display: block;
                    padding: 15px 20px;
                    text-align: center;
                    text-decoration: none;
                    color: #a855f7;
                    font-weight: 600;
                    transition: all 0.3s;
                ">
                    Xem tất cả ${products.length} kết quả
                    <i class="fas fa-arrow-right" style="margin-left: 5px;"></i>
                </a>
            `;
        }
        
        searchResults.innerHTML = resultsHTML;
        
        // Add hover effects
        const items = searchResults.querySelectorAll('.live-search-item');
        items.forEach(item => {
            item.addEventListener('mouseenter', function() {
                this.style.background = '#1a1a1a';
                this.querySelector('.fa-arrow-right').style.color = '#a855f7';
                this.querySelector('.fa-arrow-right').style.transform = 'translateX(5px)';
            });
            item.addEventListener('mouseleave', function() {
                this.style.background = 'transparent';
                this.querySelector('.fa-arrow-right').style.color = '#555';
                this.querySelector('.fa-arrow-right').style.transform = 'translateX(0)';
            });
        });
    }
    
    // Close search results when clicking outside
    document.addEventListener('click', function(e) {
        if (!searchContainer.contains(e.target)) {
            searchResults.style.display = 'none';
        }
    });
    
    // Close on Escape key
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            searchResults.style.display = 'none';
        }
    });
    
    // Reopen results when focusing on input with existing value
    searchInput.addEventListener('focus', function() {
        if (this.value.trim().length >= 2 && searchResults.children.length > 0) {
            searchResults.style.display = 'block';
        }
    });
    
    // On form submit, close dropdown and let it go to products page
    searchForm.addEventListener('submit', function(e) {
        // Hide dropdown
        searchResults.style.display = 'none';
        // Let form submit normally to show results on products page
    });
    
    // Custom scrollbar for results
    const style = document.createElement('style');
    style.textContent = `
        .live-search-results::-webkit-scrollbar {
            width: 8px;
        }
        .live-search-results::-webkit-scrollbar-track {
            background: #0a0a0a;
            border-radius: 4px;
        }
        .live-search-results::-webkit-scrollbar-thumb {
            background: #2a2a2a;
            border-radius: 4px;
        }
        .live-search-results::-webkit-scrollbar-thumb:hover {
            background: #a855f7;
        }
    `;
    document.head.appendChild(style);
});
