//************************************************************************
//
//    This work contains valuable confidential and proprietary
//    information.  Disclosure, use or reproduction without the
//    written authorization of the company is prohibited.  This
//    unpublished work by the company is protected by the laws of the
//    United States and other countries.  If publication of the work
//    should occur the following notice shall apply:
//    Copyright Baldwin 1998/2018 All Rights Reserved
//
//    Creation Date: 07/30/2018
//    Description: TDequeConcurrent for unique blocking queue
//
//************************************************************************
#pragma once


#ifndef CONCURRENT_DEQUE_H
#define CONCURRENT_DEQUE_H

#include <deque>
#include <mutex>
#include <set>
#include <condition_variable>


template< typename T >
class TDequeConcurrent {

public:
	void push(T const& value) 
	{
		{
			std::unique_lock<std::mutex> lock(this->m_mutex);
			auto element = m_collectionLookup.find(value);
			if (element != end(m_collectionLookup))
			{
				// is already inserted
				return;
			}
			m_collection.push_front(value);
			m_collectionLookup.insert(value);
		}
		this->m_condNewData.notify_one();
	}

	T pop() 
	{
		std::unique_lock<std::mutex> lock(this->m_mutex);
		this->m_condNewData.wait(lock, [=] { return !this->m_collection.empty(); });

		T rc(std::move(this->m_collection.back()));
		m_collectionLookup.erase(rc);
		this->m_collection.pop_back();
		return rc;
	}

	//! \brief Clears the deque
	void clear(void)
	{
		std::lock_guard<std::mutex> lock(m_mutex);
		m_collection.clear();
		m_collectionLookup.clear();
	}

	void ClearAndUnblock(void) noexcept
	{
		std::lock_guard<std::mutex> lock(m_mutex);
		m_collection.clear();
		m_collectionLookup.clear();
		m_condNewData.notify_all();

	}

private:

	std::deque<T> m_collection;              // Concrete, not thread safe, storage.
	std::set<T> m_collectionLookup;			 // for uniqueness 
	std::mutex    m_mutex;                   // Mutex protecting the concrete storage
	std::condition_variable m_condNewData;   // Condition used to notify that new data are available.
};


#endif // CONCURRENT_DEQUE_H
